Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'M_PublishNews')
BEGIN
	DROP  Procedure  M_PublishNews
END

GO
/***********************************************************
过程名称： M_PublishNews
功能描述： 收藏新闻
参数说明：	 
编写日期： 2016/6/6
程序作者： Allen
调试记录： exec M_PublishNews 100000002,1,6128258090
			
************************************************************/
CREATE PROCEDURE [dbo].[M_PublishNews]
	@IsPublish int=1,
	@NewsCode bigint
AS
	if(@IsPublish=1)
	begin
		Update NEWS_MAIN set IS_ISSUE=@IsPublish,PUB_TIME=GETDATE() where NEWS_UNI_CODE=@NewsCode
	end
	else
	begin
		Update NEWS_MAIN set IS_ISSUE=@IsPublish where NEWS_UNI_CODE=@NewsCode
	end
