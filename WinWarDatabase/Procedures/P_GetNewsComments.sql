Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'P_GetNewsComments')
BEGIN
	DROP  Procedure  P_GetNewsComments
END

GO
/***********************************************************
过程名称： P_GetNewsComments
功能描述： 获取新闻评论
参数说明：	 
编写日期： 2016/6/6
程序作者： Allen
调试记录： declare @out bigint=0  exec P_GetNewsComments 6128258538,0,20,@out output; print  @out
			
************************************************************/
CREATE PROCEDURE [dbo].[P_GetNewsComments]
	@UserID bigint=0,
	@NewsCode bigint,
	@PageSize int,
	@ID bigint=0 output
AS

declare @CommandSQL nvarchar(4000)
--declare @Temp table(ID bigint,[User_ID] bigint,[User_Name] nvarchar(200),Content nvarchar(4000),Reply_Count int,Praise_Count int,Create_Date datetime)  	
 if(@ID=0)
 begin
	set @CommandSQL='select top '+str(@PageSize)+' ID,[User_ID],[User_Name],Content,Reply_Count,Praise_Count,Create_Date from  NEWS_Comment where [Type]=1 and  News_Uni_Code='+str(@NewsCode)+' order by ID desc ' 
 end
 else
 begin
	set @CommandSQL='select top '+str(@PageSize)+' ID,[User_ID],[User_Name],Content,Reply_Count,Praise_Count,Create_Datefrom  NEWS_Comment where [Type]=1 News_Uni_Code='+str(@NewsCode)+' and ID<'+str(@ID)+' order by ID desc '
 end

 exec (@CommandSQL)
-- insert into @Temp exec (@CommandSQL)

--select t.*,isnull(f.ID,0) Is_Praise from @Temp t left join  Comment_Favorite f on t.ID=f.Comment_ID and f.[User_ID]=@UserID
 
select @NewsCode=min(ID) from @Temp

