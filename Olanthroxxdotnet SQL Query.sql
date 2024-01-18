USE Olanthroxxx

select  * From tblProduct
Select * from tblForgotPasswordHistory
Select * from tblUserDetails
Select * from tblUser
select * from tblProductReview
select  * From tblProduct


select * from tblMovieBookingHistory
select * from tblMovieDetails
select * from tblTheaterDetails
select * from tblMovieBookingHistory
select * from tblMovieScreenTimingConfig where ScreenTimingConfigID = 10003

select a.Address1, a.Address2 from tblAddress as a INNER JOIN tblOrderDetails as b on a.AddressID = b.BillingAddressID_FK

--UPDATE tblMovieScreenTimingConfig SET MovieDate = '2024-01-18' where ScreenTimingConfigID = 10003