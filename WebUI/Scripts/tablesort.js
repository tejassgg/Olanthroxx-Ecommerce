(function ($) {
    $.fn.sortByColumn = function (options) {      //
        // Sorts an HTML table on a column identified by the jQuery text() value of the first cell in the column
        //
        // Sort is stable.
        //
        // If the column header is not a string value, the sort will restore the original order.
        //
        // e.g.
        //    // Attach "SortbyColumn" to each column header of the table.
        //    // Header row can be in a "thead" with "th" cells or just in a "tr" with "td" cells.
        //
        // <script src="js/tablesort.min.js" type="text/javascript"></script>
        //
        // Attach sort to every header cell (note that you need to specify "th" or "td", depending on the table HTML.
        //
        //   $('#sample th').mousedown(function (e) {
        //      $('#sample').sortByColumn($(this).text());
        //    });
        //
        // Restore original order.
        //
        // $('#sample').sortByColumn();
        //
        // Sort explicitly:
        //
        //   $('#sample').sortByColumn('Part Number');
        //
        // First sort on a column is ascending; the second is descending.
        // This is reset if a different column is sorted in the interim.
        //
        // SortByColumn recognizes Date columns [mm/dd/yy[yy], dd/mm/yy[yy] and yy[yy]/mm/dd] and Numeric columms
        // and sorts accordingly.
        //
        // Columns must contain data of the same type in every row to be sorted
        // as Dates or Numbers.
        //
        // Options
        //
        // Instead of passing in the column header text, you can pass in an object literal that specifies the column header text, date format, decimal format
        // and whether string sorts are case sensitive.

        // The options are:
        //    columnText
        //    dateFormat
        //    dateDelimiter
        //    decimalDelimiter
        //    caseSensitive
        //    ascendingClass
        //    descendingClass
        //
        // "columnText" must match the text in the first cell of a table column.
        // e.g.
        //    $('#SampleTable').sortByColumn({ columnText: $(this).text()});
        //
        // "dateFormat" can be any value but only "'US'", "'UK'" and "'EU'" are recognized.
        // The default is "''" and the Plugin will determine which date format to use based on the contents of the column.
        // e.g.
        //    $('#SampleTable').sortByColumn({ columnText: $(this).text(), dateFormat: 'UK'});
        //
        // "caseSensitive" can be set to "true" or "false". The default is "true".
        // e.g.
        //    $('#SampleTable').sortByColumn({ columnText: $(this).text(), caseSensitive: false});
        //
        // "ascending" is an optional class name that will be applied to the header cell of the current sort column if it is in ascending order.
        // "descending" is an optional class name that will be applied to the header cell of the current sort column if it is in descending order.
        // e.g.
        //    $('#SampleTable').sortByColumn({ columnText: $(this).text(), ascendingClass: 'redbordertop', descendingClass: 'redborderbottom'});
        //
        //
        var settings = $.extend({
            columnText: '',
            dateFormat: '',
            dateDelimiter: '/',
            decimalDelimiter: '.',
            caseSensitive: true,
            ascendingClass: '',
            descendingClass: ''
        }, options);
        if (typeof options == 'string') {
            settings.columnText = options;
        }
        //
        // Remove any whitespace from columnText
        settings.columnText = RemoveLineBreaksTrim(settings.columnText);
        var tableData = new Array();
        var colX = -1;
        var rowX = -1;
        var sortCol = -1;
        var sortAsc = true;
        var firstRowSel = this.find('tr:first th').length > 0 ? ' tr:first th' : ' tr:first td';
        //
        // If first time, add original sequence number to each row for stable sorting
        var rownum = 0;
        this.find('tr').not(':first').each(function () {
            if ($(this).data('_seq_')) {
                return false;
            }
            $(this).data('_seq_', rownum);
            rownum++;
        });
        //
        //
        this.find(firstRowSel).each(function () {
            colX++;
            if ($(this).hasClass(settings.ascendingClass)) {
                $(this).removeClass(settings.ascendingClass);
            }
            if ($(this).hasClass(settings.descendingClass)) {
                $(this).removeClass(settings.descendingClass);
            }
            if (RemoveLineBreaksTrim($(this).text()) == settings.columnText) {
                if ($(this).data('_dir_') == 'a') {
                    $(this).data('_dir_', 'd');
                    if (settings.descendingClass) {
                        $(this).addClass(settings.descendingClass);
                    }
                }
                else {
                    $(this).data('_dir_', 'a');
                    if (settings.ascendingClass) {
                        $(this).addClass(settings.ascendingClass);
                    }
                }
                sortCol = colX;
                sortAsc = ($(this).data('_dir_') == 'a');
            }
            else {
                $(this).data('_dir_', '');
            }
        });

        var RowData = function () {
            this.Seq = 0;
            this.Key = '';
            this.KeyType = 0;  // 0 = string, 1 = number, 2 = date (mm/dd/yy[yy]), 3 - date (dd/mm/yy[yy]), 4 - date (yy[yy]/mm/dd)
            this.Cells = new Array();
        }
        var prevType = -1;
        this.find('tr').not(':first').each(function () {
            //
            // Skip Footers
            if (!$(this).parent().is('tfoot')) {
                var rowData = new RowData();
                var numExp = new RegExp('[^0-9' + settings.decimalDelimiter + '-]+', 'g');
                rowData.Seq = $(this).data('_seq_');
                colX = -1;
                $(this).children().each(function () {
                    colX++;
                    var cellValue = RemoveLineBreaksTrim($(this).text());
                    if (colX == sortCol) {
                        rowData.Key = cellValue;
                        if (prevType == 0) {
                            rowData.KeyType = 0;
                        }
                        else {
                            rowData.KeyType = GetType(cellValue, numExp, settings.dateDelimiter, settings.decimalDelimiter);
                        }
                        if (rowData.KeyType == 1) {
                            rowData.Key = cellValue.replace(numExp, '').replace(settings.decimalDelimiter, '.');
                        }
                        else {
                            rowData.Key = (rowData.KeyType == 0 && !settings.caseSensitive) ? cellValue.toUpperCase() : cellValue;
                        }
                        prevType = rowData.KeyType;
                    }
                    rowData.Cells.push(cellValue);
                });
                tableData.push(rowData);
            }
        });
        //
        // Check data types consistent within a column. If there
        // is a conflict, set column to string type (0).
        if (tableData.length > 1) {
            for (rowX = 0 ; rowX < tableData.length - 1; rowX++) {
                if (tableData[rowX].KeyType != tableData[rowX + 1].KeyType) {
                    for (var rowY = 0; rowY < tableData.length; rowY++) {
                        tableData[rowY].KeyType = 0;
                    }
                    break;
                }
            }
            //
            // Resolve dates if dateFormat not specified
            for (colX = 0; colX < tableData[0].Cells.length; colX++) {
                if (colX == sortCol) {
                    if (tableData[0].KeyType == 2) {
                        switch (settings.dateFormat) {
                            case 'US':
                            tableData[0].KeyType = 2;
                            break;
                            case 'UK':
                            tableData[0].KeyType = 3;
                            break;
                            case 'EU':
                            tableData[0].KeyType = 4;
                            break;
                            default:
                            tableData[0].KeyType = 0;
                            break;
                        }
                        // Date format not specified. Figure it out from the data in the column.
                        if (tableData[0].KeyType == 0) {
                            var validDays = [0, 0, 0];
                            var validMonths = [0, 0, 0];
                            var validYears = [0, 0, 0];
                            for (var rowX = 0; rowX < tableData.length; rowX++) {
                                var parts = tableData[rowX].Cells[colX].split(settings.dateDelimiter);
                                SetStats(parts, 0, validDays, validMonths, validYears);
                                SetStats(parts, 1, validDays, validMonths, validYears);
                                SetStats(parts, 2, validDays, validMonths, validYears);
                            }
                            var monthPos = MaxIndex(validMonths);
                            validYears[monthPos] = -1;
                            validDays[monthPos] = -1;
                            var dayPos = MaxIndex(validDays);
                            validYears[dayPos] = -1;
                            var yearPos = MaxIndex(validYears);

                            if (monthPos == 0 && dayPos == 1 && yearPos == 2) {
                                tableData[0].KeyType = 2; // US
                            }
                            else if (monthPos == 1 && dayPos == 0 && yearPos == 2) {
                                tableData[0].KeyType = 3; // UK
                            }
                            else if (monthPos == 1 && dayPos == 2 && yearPos == 0) {
                                tableData[0].KeyType = 4; // EU
                            }
                            else {
                                tableData[0].KeyType = 0; // Give up - treat as string
                            }
                        }
                        // Propogate KeyType to all rows
                        for (var rowX = 1; rowX < tableData.length; rowX++) {
                            tableData[rowX].KeyType = tableData[0].KeyType;
                        }
                        break;
                    }
                }
            }
        }

        if (settings.columnText) {
            tableData.sort(function (a, b) {
                switch (a.KeyType) {
                    case 0: // string
                    if (a.Key > b.Key) {
                        return sortAsc ? 1 : -1;
                    }
                    if (a.Key < b.Key) {
                        return sortAsc ? -1 : 1;
                    }
                    break;
                    case 1: // Numeric
                    if (parseFloat(a.Key) > parseFloat(b.Key)) {
                        return sortAsc ? 1 : -1;
                    }
                    if (parseFloat(a.Key) < parseFloat(b.Key)) {
                        return sortAsc ? -1 : 1;
                    }
                    break;
                    default: // Date
                    var res = DateCompare(a.KeyType, a.Key, b.Key, settings.dateDelimiter);
                    if (res == 1) {
                        return sortAsc ? 1 : -1;
                    }
                    if (res == -1) {
                        return sortAsc ? -1 : 1;
                    }
                }
                if (a.Seq > b.Seq) {
                    return 1;
                }
                return -1;
            });
        }
        else {
            tableData.sort(function (a, b) {
                if (a.Seq > b.Seq) {
                    return 1;
                }
                return -1;
            });
        }
        rowX = -1;
        this.find('tr').not(':first').each(function () {
            if (!$(this).parent().is('tfoot')) {
                rowX++;
                var rowData = tableData[rowX];
                $(this).data('_seq_', rowData.Seq);
                colX = -1;
                $(this).children().each(function () {
                    colX++;
                    $(this).text(rowData.Cells[colX]);
                });
            }
        });
        return this;
    }
    var SetStats = function (parts, i, validDays, validMonths, validYears) {
        if (parts[i] > 0 && parts[i] <= 12) {
            validMonths[i]++;
            validDays[i]++;
            validYears[i]++;
        }
        else if (parts[i] > 0 && parts[i] <= 31) {
            validDays[i]++;
            validYears[i]++;
        }
        else {
            validYears[i]++;
        }
    }
    var MaxIndex = function (validPart) {
        var maxValue = -1;
        var maxIndex = 0;
        for (var i = validPart.length - 1; i >= 0; i--) {
            if (validPart[i] > maxValue) {
                maxValue = validPart[i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }
    var GetType = function (cellValue, numExp, dateDelimiter, decimalDelimiter) {
        var numValue = cellValue.replace(numExp, '').replace(decimalDelimiter, ".");
        if (cellValue.indexOf(dateDelimiter) == -1 && parseFloat(numValue)) {
            return 1;
        }
        var parts = cellValue.split(dateDelimiter);
        if (parts.length != 3) {
            return 0;
        }
        var dd = parseInt(parts[1]);
        if (isNaN(dd)) {
            return 0;
        }
        var mm = parseInt(parts[0]);
        if (isNaN(mm)) {
            return 0;
        }
        var yyyy = parseInt(parts[2]);
        if (isNaN(yyyy)) {
            return 0;
        }
        return 2;
    }
    var DateCompare = function (keyType, dateA, dateB, dateDelimiter) {
        // 2 = date (mm/dd/yy[yy]), 3 - date (dd/mm/yy[yy]), 4 - date (yy[yy]/mm/dd)
        var yearPos = 2;
        var monthPos = 0;
        var dayPos = 1;
        switch (keyType) {
            case 3:
            yearPos = 2;
            monthPos = 1;
            dayPos = 0;
            break;

            case 4:
            yearPos = 0;
            monthPos = 1;
            dayPos = 2;
            break;
        }
        var partsA = dateA.split(dateDelimiter);
        var partsB = dateB.split(dateDelimiter);
        // Years
        if (parseInt(partsA[yearPos]) > parseInt(partsB[yearPos])) {
            return 1;
        }
        if (parseInt(partsA[yearPos]) < parseInt(partsB[yearPos])) {
            return -1;
        }
        // Months
        if (parseInt(partsA[monthPos]) > parseInt(partsB[monthPos])) {
            return 1;
        }
        if (parseInt(partsA[monthPos]) < parseInt(partsB[monthPos])) {
            return -1;
        }
        // Days
        if (parseInt(partsA[dayPos]) > parseInt(partsB[dayPos])) {
            return 1;
        }
        if (parseInt(partsA[dayPos]) < parseInt(partsB[dayPos])) {
            return -1;
        }
        return 0;
    }
    var RemoveLineBreaksTrim = function (htmlString) {
        htmlString = htmlString.replace(/(\r\n|\n|\r)/gm, " ");
        return htmlString.trim();
    }
}(jQuery));
