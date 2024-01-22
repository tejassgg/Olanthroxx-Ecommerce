USE Olanthroxxx

select  * From tblProduct
Select * from tblForgotPasswordHistory
Select * from tblUserDetails
Select * from tblUser
select * from tblProductReview
select  * From tblProduct

select * from tblTempCartDetails


select * from tblMovieBookingHistory
select * from tblMovieDetails
select * from tblTheaterDetails
select * from tblMovieBookingHistory
select * from tblMovieScreenTimingConfig where ScreenTimingConfigID = 10003

select a.Address1, a.Address2 from tblAddress as a INNER JOIN tblOrderDetails as b on a.AddressID = b.BillingAddressID_FK

--UPDATE tblMovieScreenTimingConfig SET MovieDate = '2024-01-18' where ScreenTimingConfigID = 10003

--INSERT INTO tblTempCartDetails VALUES (10010,'[{"ProductID":16,"Quantity":1,"Amount":36,"Price":36,"Name":"Organic Potato"},{"ProductID":23,"Quantity":2,"Amount":130,"Price":65,"Name":"Organic Tomato"}]', 'xxeykg1b2xxtom15ugswz5h4',0, GETDATE(), NULL )


