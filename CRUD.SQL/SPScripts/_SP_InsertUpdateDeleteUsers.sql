USE [Training]
GO
/****** Object:  StoredProcedure [dbo].[_SP_InsertUpdateDeleteUsers]    Script Date: 30-12-2022 02:34:27 PM ******/

-- =============================================      

-- Author:  Raj Gondaliya
-- Create date: 30-12-2022
-- Description: Insert, Update And Delete Users

/*
EXEC _SP_InsertUpdateDeleteUsers
	@ID = NULL,
	@First_Name = '',
	@Last_Name = '',
	@Phone_Number = '',
	@Email = '',
	@Gender = '',
	@Operation = ''
*/
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[_SP_InsertUpdateDeleteUsers]
	(
		@ID int = NULL,
		@FirstName varchar(20),
		@LastName varchar(20),
		@PhoneNumber varchar(15),
		@Email nvarchar(100),
		@Gender char(10),
		@Operation int
	)
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRY
	
	IF @Operation = 1
    BEGIN
		INSERT INTO [Users] VALUES (@FirstName,@LastName,@PhoneNumber,@Email,@Gender)
	END
	ELSE
	BEGIN
		
		IF @Operation = 2
		BEGIN
			UPDATE [Users] SET [FirstName] = @FirstName, [LastName] = @LastName, [PhoneNumber] = @PhoneNumber, Email = @Email, Gender = @Gender WHERE ID = @ID
		END
		
		ELSE
		BEGIN
			DELETE FROM [Users] WHERE ID = @ID
		END
	END

	END TRY
	
	BEGIN CATCH
	
	DECLARE @ErrMsg VARCHAR(MAX)
	
	SELECT @ErrMsg = ERROR_MESSAGE()

	RAISERROR('Error in _SP_InsertUpdateDeleteUsers PROC: %s', 15, 1, @ErrMsg)
	
	END CATCH
END
