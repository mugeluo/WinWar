Use [WWXW]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'P_AddNewsCommentPraiseCount')
BEGIN
	DROP  Procedure  P_AddNewsCommentPraiseCount
END

GO
/***********************************************************
过程名称： P_AddNewsCommentPraiseCount
功能描述： 点赞评论
参数说明：	 
编写日期： 2016/7/23
程序作者： Allen
调试记录： exec P_AddNewsCommentPraiseCount 100000002,1,6128258090
			
************************************************************/
CREATE PROCEDURE [dbo].[P_AddNewsCommentPraiseCount]
	@UserID bigint,
	@IsAdd int=1,
	@ID bigint
AS

if(@UserID>0 and @ID>0)
begin
	if (@IsAdd=0 and exists(select ID from Comment_Favorite where Comment_ID=@ID and [User_ID]=@UserID))
	begin
		delete from Comment_Favorite where Comment_ID=@ID and [User_ID]=@UserID

		Update NEWS_Comment set Praise_Count=Praise_Count-1 where ID=@ID and Praise_Count>0
	end
	else if(@IsAdd=1 and not exists(select ID from Comment_Favorite where Comment_ID=@ID and [User_ID]=@UserID))
	begin
		insert into Comment_Favorite([User_ID],Comment_ID,Create_Date)
		values(@UserID,@ID,getdate())

		Update NEWS_Comment set Praise_Count=Praise_Count+1 where ID=@ID
	end
end
