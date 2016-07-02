Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'M_EditNews')
BEGIN
	DROP  Procedure  M_EditNews
END

GO
/***********************************************************
过程名称： M_EditNews
功能描述： 添加评论
参数说明：	 
编写日期： 2016/6/6
程序作者： Allen
调试记录： exec M_EditNews 100000002,1,6128258538
			
************************************************************/
CREATE PROCEDURE [dbo].[M_EditNews]
	@ID bigint,
	@Title nvarchar(500),
	@TitleSub nvarchar(500)='',
	@TitleApp nvarchar(500)='',
	@NewsSum nvarchar(500)='',
	@Author nvarchar(100)='',
	@Source nvarchar(100)='',
	@PicUrl nvarchar(200)='',
	@PosiPar int=3,
	@Important int=2,
	@IsIssue int=0,
	@TypeID bigint,
	@Content text
AS


update NEWS_MAIN set TITLE_MAIN=@Title,TITLE_SUB=@TitleSub,TITLE_APP=@TitleApp,Pic_URL=@PicUrl,
NEWS_SUM=@NewsSum,NEWS_AUTHOR=@Author,REAL_SOURCE_NAME=@Source,NEGA_POSI_PAR=@PosiPar,IMPT_PAR=@Important,IS_ISSUE=@IsIssue,NEWS_TYPE=@TypeID
where NEWS_UNI_CODE=@ID

update  NEWS_CONTENT set HTML_TXT=@Content
where NEWS_UNI_CODE=@ID

