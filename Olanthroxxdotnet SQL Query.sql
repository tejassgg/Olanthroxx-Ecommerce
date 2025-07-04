USE Olanthroxxx

select * from tblUserSessionDetails
select * from tblLogs

select  * From tblProduct
Select * from tblForgotPasswordHistory
Select * from tblUserDetails order by AccountID_FK
Select * from tblUser order by AccountID
select * from tblProductReview
select  * From tblProduct where SellerName = 'cooldude69'

--UPDATE tblUserDetails Set City = 'Pune', State = 'Maharashtra', PinCode = '410014' where AccountID_FK = '10035'

--INSERT into tblUserDetails values (10008, 'walmart', '', '', '6504047707', 'tejassgg1@gmail.com', '', 'Maharashtra', 'Thane', '400072', 'Type H1,Build No. 15,5th Floor,Udaygiri  IIT Campus.Powai, Mumbai-76', '2023-01-22', null )

--UPDATE tblProduct SET ItemsSold = 25 where ProductID = 1

select SUM(Quantity) from tblOrderDetails where ProductID = 24

Select * from tblOrderDetails
select * from tblTempCartDetails

select  * From tblProduct
select * from tblOrderDetails where OrderID = '22804605-5579-4f99-b015-08ac49fb2290' and OrderStatus !=4

--UPDATE tblOrderDetails Set OrderStatus = 4 WHERE OrderID = 'ff52bc8f-fcc9-4dc2-9920-652bdf4f4fad' and ProductID = 6
--UPDATE tblUser SET IsActive = 0 where UserName in ('sgg1400', 'tvirangana')

select * from tblTheaterDetails
select * from tblMovieDetails
Select * from tblScreenDetails
select * from tblMovieScreenTimingConfig
Select * from tblMasCommonType WHERE MasterType = 'MovieTimingFrom' ORDER by Code Asc
select * from tblMovieBookingHistory

--DELETE FROM tblTheaterDetails where TheaterID = 1007
	
select a.Address1, a.Address2 from tblAddress as a INNER JOIN tblOrderDetails as b on a.AddressID = b.BillingAddressID_FK

--UPDATE tblMovieScreenTimingConfig SET MovieDate = '2024-09-10' where ScreenTimingConfigID = 10003
--UPDATE tblTheaterDetails SET ImgPath = '~/Content/Images/Theatres/tejassgg-HumaCinemas-5968.jpg' where TheaterID = 1000

--INSERT INTO tblTempCartDetails VALUES (10010,'[{"ProductID":16,"Quantity":1,"Amount":36,"Price":36,"Name":"Organic Potato"},{"ProductID":23,"Quantity":2,"Amount":130,"Price":65,"Name":"Organic Tomato"}]', 'xxeykg1b2xxtom15ugswz5h4',0, GETDATE(), NULL )

--Alter TABLE tblTheaterDetails ADD ImgPath nvarchar(200); 
--ALTER TABLE tblOrderDetails ADD OTP int
--ALTER TABLE tblOrderDetails ADD OTPToBeShown bit
--ALTER TABLE tblProduct ADD ItemsSold int
	

select * from tblOrderDetails as a 
JOIN tblProduct as b on b.ProductID = a.ProductID
where b.SellerName = 'walmart'
and a.OrderID = '1ae45f92-aec6-4112-9743-e9d2b0af2cc1'