Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'P_GetNEWS_Favorites')
BEGIN
	DROP  Procedure  P_GetNEWS_Favorites
END

GO
/***********************************************************
过程名称： P_GetNEWS_Favorites
功能描述： 获取我收藏的新闻列表
参数说明：	 
编写日期： 2016/6/6
程序作者： Allen
调试记录： declare @out bigint=0  exec P_GetNEWS_Favorites 111111,1,@out output; print  @out
			
************************************************************/
CREATE PROCEDURE [dbo].[P_GetNEWS_Favorites]
	@UserID bigint=0,
	@PageSize int,
	@FavoriteID bigint=0 output
AS
declare @CommandSQL nvarchar(4000)
declare @Temp table(News_Uni_Code bigint,Pub_Time datetime,News_Author nvarchar(200),TITLE_MAIN  nvarchar(500),Pic_URL nvarchar(300),View_Count int,Comment_Count int,Praise_Count int,
				    Collect_Count int,REAL_SOURCE_NAME nvarchar(200),NEWS_TYPE nvarchar(500),TITLE_SUB  nvarchar(500),FavoriteID bigint)  

set @CommandSQL='select top '+str(@PageSize)+' n.News_Uni_Code,Pub_Time,News_Author,TITLE_MAIN,Pic_URL,View_Count,Comment_Count,Praise_Count,Collect_Count,
				 REAL_SOURCE_NAME,NEWS_TYPE,TITLE_SUB,f.ID  FavoriteID from  
				NEWS_Favorite as f join  NEWS_MAIN as n on f.NEWS_UNI_CODE=n.NEWS_UNI_CODE 
				where n.IS_ISSUE=''1'' and f.Is_Collect=1 and f.USER_ID='+str(@UserID)
if(@FavoriteID>0)
	set @CommandSQL+=' and f.ID>'+str(@FavoriteID)

set @CommandSQL+=' order by f.ID desc'

insert into @Temp exec (@CommandSQL)

select * from @Temp

select @FavoriteID=min(FavoriteID) from @Temp


