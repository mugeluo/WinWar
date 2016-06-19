Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'P_GetNewsDetail')
BEGIN
	DROP  Procedure  P_GetNewsDetail
END

GO
/***********************************************************
过程名称： P_GetNewsDetail
功能描述： 获取新闻详情
参数说明：	 
编写日期： 2016/6/９
程序作者： Ｍu
调试记录： exec P_GetNewsDetail 6128258538
			
************************************************************/
CREATE PROCEDURE [dbo].[P_GetNewsDetail]
	@NewsCode bigint,
	@UserID bigint=0
AS

if(@UserID<>0)
	select ｎ.*,isnull(f.Is_Praise,0) Is_Praise,isnull(Is_Collect,0) Is_Collect,c.HTML_TXT from NEWS_MAIN ｎ 
	left join  NEWS_CONTENT c on ｎ.News_Uni_Code=c.News_Uni_Code 
	left join  NEWS_Favorite f on ｎ.News_Uni_Code=f.News_Uni_Code and f.[User_ID]=@UserID
	where ｎ.News_Uni_Code=@NewsCode 
else
	select ｎ.*,c.HTML_TXT from NEWS_MAIN ｎ 
	left join  NEWS_CONTENT c on ｎ.News_Uni_Code=c.News_Uni_Code
	where ｎ.News_Uni_Code=@NewsCode 

update NEWS_MAIN set View_Count=View_Count+1 where News_Uni_Code=@NewsCode
 


