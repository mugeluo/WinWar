Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'P_AddNewsComment')
BEGIN
	DROP  Procedure  P_AddNewsComment
END

GO
/***********************************************************
过程名称： P_AddNewsComment
功能描述： 添加评论
参数说明：	 
编写日期： 2016/6/6
程序作者： Allen
调试记录： exec P_AddNewsComment 100000002,1,6128258538
			
************************************************************/
CREATE PROCEDURE [dbo].[P_AddNewsComment]
	@NewsCode bigint,
	@Content nvarchar(500)='',
	@UserID bigint,
	@UserName nvarchar(100)='',
	@ReplyID bigint=0,
	@ReplyUserID bigint,
	@ReplyUserName nvarchar(100)=''
AS

if(@ReplyID=0)
begin
	insert into NEWS_Comment([User_ID],[User_Name],NEWS_UNI_CODE,[Type],Content,Praise_Count,Reply_Count,Reply_ID,Reply_User_ID,Reply_User_Name,Create_Date)
	values(@UserID,@UserName,@NewsCode,@Content,1,0,0,0,0,'',getdate())
end
else
begin
	insert into NEWS_Comment([User_ID],[User_Name],NEWS_UNI_CODE,[Type],Content,Praise_Count,Reply_Count,Reply_ID,Reply_User_ID,Reply_User_Name,Create_Date)
	select @UserID,@UserName,@NewsCode,@Content,2,0,0,@ReplyID,@ReplyUserID,@ReplyUserName,getdate() from NEWS_Comment where ID=@ReplyID

	Update NEWS_Comment set Reply_Count=Reply_Count+1 where ID=@ReplyID
end

Update NEWS_MAIN set Comment_Count=Comment_Count+1 where NEWS_UNI_CODE=@NewsCode

