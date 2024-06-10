USE Olanthroxxx

select  * From tblProduct
Select * from tblForgotPasswordHistory
Select * from tblUserDetails
Select * from tblUser
select * from tblProductReview
select  * From tblProduct

Select * from tblOrderDetails
select * from tblTempCartDetails

select  * From tblProduct
select * from tblOrderDetails where OrderID = '80ce43d6-9655-413b-a399-01cc5691b47d' and OrderStatus !=4


select * from tblTheaterDetails
select * from tblMovieDetails
Select * from tblScreenDetails
select * from tblMovieScreenTimingConfig
Select * from tblMasCommonType WHERE MasterType = 'MovieTimingFrom' ORDER by Code Asc
select * from tblMovieBookingHistory

--DELETE FROM tblTheaterDetails where TheaterID = 1007
	
select a.Address1, a.Address2 from tblAddress as a INNER JOIN tblOrderDetails as b on a.AddressID = b.BillingAddressID_FK

--UPDATE tblMovieScreenTimingConfig SET MovieDate = '2024-06-10' where ScreenTimingConfigID = 10003
--UPDATE tblTheaterDetails SET ImgPath = '~/Content/Images/Theatres/tejassgg-HumaCinemas-5968.jpg' where TheaterID = 1000

--INSERT INTO tblTempCartDetails VALUES (10010,'[{"ProductID":16,"Quantity":1,"Amount":36,"Price":36,"Name":"Organic Potato"},{"ProductID":23,"Quantity":2,"Amount":130,"Price":65,"Name":"Organic Tomato"}]', 'xxeykg1b2xxtom15ugswz5h4',0, GETDATE(), NULL )

--Alter TABLE tblTheaterDetails ADD ImgPath nvarchar(200); 
--ALTER TABLE tblOrderDetails ADD OTP int
--ALTER TABLE tblOrderDetails ADD OTPToBeShown bit
	

select * from tblOrderDetails as a 
JOIN tblProduct as b on b.ProductID = a.ProductID
where b.SellerName = 'walmart'
and a.OrderID = '1ae45f92-aec6-4112-9743-e9d2b0af2cc1'