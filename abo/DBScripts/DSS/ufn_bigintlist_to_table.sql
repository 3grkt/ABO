IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_bigintlist_to_table]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ufn_bigintlist_to_table]
GO

-- =============================================
-- Author:		Tri Nguyen
-- Description:	Convert list of bigint to table
-- Log:
--		03/21/2013: created
-- =============================================
CREATE FUNCTION [dbo].[ufn_bigintlist_to_table] (@list varchar(MAX))
   RETURNS @tbl TABLE (number bigint NOT NULL) AS
BEGIN
   DECLARE @pos        int,
           @nextpos    int,
           @valuelen   int

   SELECT @pos = 0, @nextpos = 1

   WHILE @nextpos > 0
   BEGIN
      SELECT @nextpos = charindex(',', @list, @pos + 1)
      SELECT @valuelen = CASE WHEN @nextpos > 0
                              THEN @nextpos
                              ELSE len(@list) + 1
                         END - @pos - 1
      INSERT @tbl (number)
         VALUES (convert(bigint, substring(@list, @pos + 1, @valuelen)))
      SELECT @pos = @nextpos
   END
  RETURN
END

GO


