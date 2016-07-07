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
	@BigTypeID int=-1,
	@TypeID bigint=0,
	@PublishStatus int=-1,
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
	
	set @tableName='NEWS_MAIN'
	set @columns='*'
	set @key='News_Uni_Code'
	set @orderColumn='PUB_TIME desc'
	set @condition=' 1=1 '

	if(@KeyWords<>'')
	begin
		set @condition+=' and ( TITLE_MAIN like ''%'+@KeyWords+'%'' or TITLE_SUB like ''%'+@KeyWords+'%'' or TITLE_APP like ''%'+@KeyWords+'%'' or NEWS_AUTHOR like ''%'+@KeyWords+'%'') '
	end

	if(@TypeID>0)
	begin
		set @condition+=' and NEWS_TYPE='+str(@TypeID)
	end
	else if(@BigTypeID>0)
	begin
		create table #Type(TypeID int)
		insert into #Type select NEWS_TYPE_2 from NEWS_TYPE where NEWS_TYPE_1=@BigTypeID and NEWS_TYPE_2 is not null and NEWS_TYPE_2<>''

		set @condition+=' and NEWS_TYPE in (select TypeID from #Type)'
	end

	if(@PublishStatus<>-1)
	begin
		set @condition+=' and Is_Issue='+str(@PublishStatus)
	end
	declare @total int,@page int
	exec P_GetPagerData @tableName,@columns,@condition,@key,@OrderColumn,@pageSize,@pageIndex,@total out,@page out,0 
	select @totalCount=@total,@pageCount =@page




