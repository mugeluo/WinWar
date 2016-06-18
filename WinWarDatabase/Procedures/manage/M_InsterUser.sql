Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'M_InsterUser')
BEGIN
	DROP  Procedure  M_InsterUser
END

GO
/***********************************************************
过程名称： M_InsterUser
功能描述： 添加云销用户
参数说明：	 
编写日期： 2015/4/10
程序作者： Allen
调试记录： exec M_InsterUser 
************************************************************/
CREATE PROCEDURE [dbo].[M_InsterUser]
@UserID nvarchar(64),
@LoginName nvarchar(200)='',
@LoginPWD nvarchar(64)='',
@Name nvarchar(200),
@Mobile nvarchar(64)='',
@Email nvarchar(200)='',
@CityCode nvarchar(10)='',
@Address nvarchar(200)='',
@Jobs nvarchar(200)='',
@RoleID nvarchar(64)='',
@CreateUserID nvarchar(64)='',
@Result int output --0：失败，1：成功，2 账号已存在 3：人数超限
AS

begin tran

set @Result=0

declare @Err int=0,@MaxCount int=0,@Count int

 
--账号已存在
if(@LoginName<>'' and exists(select UserID from Users where LoginName=@LoginName  and Status=1) )
begin
	set @Result=2
	rollback tran
	return
end


insert into Users(UserID,LoginName,LoginPWD,Name,MobilePhone,Email,CityCode,Address,Jobs,Allocation,Status,IsDefault,RoleID,CreateUserID)
             values(@UserID,@LoginName,@LoginPWD,@Name,@Mobile,@Email,@CityCode,@Address,@Jobs,1,1,0,@RoleID,@CreateUserID)

   
--角色关系
if(@RoleID<>'')
begin
	insert into UserRole(UserID,RoleID,CreateUserID) values(@UserID,@RoleID,@CreateUserID) 
	set @Err+=@@error
end
if(@Err>0)
begin
	set @Result=0
	rollback tran
end 
else
begin
	select * from Users where UserID=@UserID
	set @Result=1
	commit tran
end