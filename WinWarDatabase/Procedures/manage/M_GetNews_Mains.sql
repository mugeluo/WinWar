Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'M_GetNews_Mains')
BEGIN
	DROP  Procedure  M_GetNews_Mains
END

GO
/***********************************************************
过程名称： M_GetNews_Mains
功能描述： 获取新闻列表
参数说明：	 
编写日期： 2016/6/13
程序作者： MU
调试记录： declare @out bigint=0  exec M_GetNews_Mains 100000002,'',0,20,@out output; print  @out
			
************************************************************/
CREATE PROCEDURE [dbo].[M_GetNews_Mains]
	@KeyWords nvarchar(400)='',
	@TypeID bigint=0,
	@PageSize int=10,
	@PageIndex int=1,
	@TotalCount int output,
	@PageCount int output
AS

declare @CommandSQL nvarchar(4000)
declare @tableName nvarchar(4000),
	@columns nvarchar(4000),
	@condition nvarchar(4000),
	@orderColumn nvarchar(100),
	@key nvarchar(100)
	
	set @tableName='NEWS_MAIN n left join NEWS_TYPE t on n.NEWS_TYPE=t.CLS_CODE '
	set @columns='n.*,t.NEWS_TYPE_NAME2'
	set @key='n.PUB_TIME'
	set @orderColumn='n.NEWS_UNI_CODE'
	set @condition=' 1=1 '

	if(@KeyWords<>'')
		set @condition+=' and ( n.TITLE_MAIN like ''%'+@KeyWords+'%'' ) '

	if(@TypeID<>0)
		set @condition+=' and n.NEWS_TYPE='+str(@TypeID)

	declare @total int,@page int
	exec P_GetPagerData @tableName,@columns,@condition,@key,@OrderColumn,@pageSize,@pageIndex,@total out,@page out,0 
	select @totalCount=@total,@pageCount =@page




