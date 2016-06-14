Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'M_AddNews')
BEGIN
	DROP  Procedure  M_AddNews
END

GO
/***********************************************************
过程名称： M_AddNews
功能描述： 添加评论
参数说明：	 
编写日期： 2016/6/6
程序作者： Allen
调试记录： exec M_AddNews 100000002,1,6128258538
			
************************************************************/
CREATE PROCEDURE [dbo].[M_AddNews]
	@ID bigint,
	@Title nvarchar(500),
	@TitleSub nvarchar(500)='',
	@TitleApp nvarchar(500)='',
	@NewsSum nvarchar(500)='',
	@Author nvarchar(100)='',
	@Source nvarchar(100)='',
	@PosiPar int=3,
	@Important int=2,
	@TypeID bigint,
	@Content text
AS
insert into NEWS_MAIN(NEWS_UNI_CODE,TITLE_MAIN,TITLE_SUB,TITLE_APP,NEWS_SUM,NEWS_AUTHOR,REAL_SOURCE_NAME,NEGA_POSI_PAR,IMPT_PAR,NEWS_TYPE)
values(@ID,@Title,@TitleSub,@TitleApp,@NewsSum,@Author,@Source,@PosiPar,@Important,@TypeID)

insert into NEWS_CONTENT(NEWS_UNI_CODE,HTML_TXT)
values(@ID,@Content)

