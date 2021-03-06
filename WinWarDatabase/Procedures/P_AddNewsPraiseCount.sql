﻿Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'P_AddNewsPraiseCount')
BEGIN
	DROP  Procedure  P_AddNewsPraiseCount
END

GO
/***********************************************************
过程名称： P_AddNewsPraiseCount
功能描述： 收藏新闻
参数说明：	 
编写日期： 2016/6/6
程序作者： Allen
调试记录： exec P_AddNewsPraiseCount 100000002,1,6128258090
			
************************************************************/
CREATE PROCEDURE [dbo].[P_AddNewsPraiseCount]
	@UserID bigint,
	@IsAdd int=1,
	@NewsCode bigint
AS

if(@UserID>0 and @NewsCode>0)
begin
	if exists(select ID from NEWS_Favorite where NEWS_UNI_CODE=@NewsCode and [User_ID]=@UserID)
	begin
		Update NEWS_Favorite set Is_Praise=@IsAdd where NEWS_UNI_CODE=@NewsCode and [User_ID]=@UserID
	end
	else
	begin
		insert into NEWS_Favorite([User_ID],NEWS_UNI_CODE,Is_Praise,Is_Collect,Create_Date)
		values(@UserID,@NewsCode,@IsAdd,0,getdate())
	end

	if(@IsAdd=1)
	begin
		Update NEWS_MAIN set Praise_Count=Praise_Count+1 where NEWS_UNI_CODE=@NewsCode
	end
	else
	begin
		Update NEWS_MAIN set Praise_Count=Praise_Count-1 where NEWS_UNI_CODE=@NewsCode and Praise_Count>=1
	end
end
