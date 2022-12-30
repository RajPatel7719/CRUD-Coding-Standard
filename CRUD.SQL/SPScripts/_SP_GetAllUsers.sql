USE [Training]
GO
/****** Object:  StoredProcedure [dbo].[_SP_GetAllUsers]    Script Date: 30-12-2022 02:38:47 PM ******/

-- =============================================      
-- Author:  Raj Gondaliya
-- Create date: 30-12-2022
-- Description: Get all list of Users
      
/*      
EXEC _SP_GetAllUsers
*/      
      
-- =============================================      

CREATE OR ALTER PROC [dbo].[_SP_GetAllUsers]
as
begin
select ID, FirstName, LastName, PhoneNumber, Email, Gender
from [Users] 
end