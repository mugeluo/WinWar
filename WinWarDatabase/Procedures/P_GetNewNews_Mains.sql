Use [WWXW_DEV]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'P_GetNewNews_Mains')
BEGIN
	DROP  Procedure  P_GetNewNews_Mains
END

GO
/***********************************************************
过程名称： P_GetNewNews_Mains
功能描述： 获取最新新闻列表
参数说明：	 
编写日期： 2016/7/31
程序作者： MU
调试记录： declare @out bigint=0  exec P_GetNewNews_Mains 100000002,'',0,20,@out output; print  @out
			
************************************************************/
CREATE PROCEDURE [dbo].[P_GetNewNews_Mains]
	@UserID bigint=0,
	@TypeID int=-1,
	@PageSize int,
	@MaxNewsCode bigint
AS

declare @CommandSQL nvarchar(4000)
declare @Temp table(News_Uni_Code bigint,Pub_Time datetime,News_Author nvarchar(200),TITLE_MAIN  nvarchar(500),Pic_URL nvarchar(300),View_Count int,Comment_Count int,Praise_Count int,
				    Collect_Count int,REAL_SOURCE_NAME nvarchar(200),NEWS_TYPE nvarchar(500),TITLE_SUB  nvarchar(500))  	

 begin
	set @CommandSQL='select top '+str(@PageSize)+' News_Uni_Code,Pub_Time,News_Author,TITLE_MAIN,Pic_URL,View_Count,Comment_Count,Praise_Count,Collect_Count,
				 REAL_SOURCE_NAME,NEWS_TYPE,TITLE_SUB from  NEWS_MAIN where IS_ISSUE=''1'' and News_Uni_Code>'+str(@MaxNewsCode)
 end

if(@TypeID>0)
begin
	set @CommandSQL+=' and NEWS_TYPE='+str(@TypeID)
end

 set @CommandSQL+=' order by News_Uni_Code desc'

 insert into @Temp exec (@CommandSQL)

select t.*,isnull(f.Is_Praise,0) Is_Praise,isnull(Is_Collect,0) Is_Collect from @Temp t left join  NEWS_Favorite f on t.News_Uni_Code=f.News_Uni_Code and f.[User_ID]=@UserID


