Use [WinWar]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'M_GetUserToLogin')
BEGIN
	DROP  Procedure  M_GetUserToLogin
END

GO
/***********************************************************
过程名称： M_GetUserToLogin
功能描述： 验证云销系统登录并返回信息
参数说明：	 
编写日期： 2015/4/22
程序作者： Allen
调试记录： exec M_GetUserToLogin 'Admin','ADA9D527563353B415575BD5BAAE0469'
************************************************************/
CREATE PROCEDURE [dbo].[M_GetUserToLogin]
@LoginName nvarchar(200),
@LoginPWD nvarchar(64),
@Result int output  --1:查询正常；2：用户名不存在；3：用户密码有误;4:用户被注销
AS


declare @UserID nvarchar(64),@ClientID nvarchar(64),@AgentID nvarchar(64),@RoleID nvarchar(64)
set @UserID=''
IF  EXISTS(select UserID from Users where LoginName=@LoginName and Status<>9)
begin

	select @UserID = UserID,@ClientID=ClientID,@AgentID=AgentID,@RoleID=RoleID from Users 
	where LoginName=@LoginName and LoginPWD=@LoginPWD

	if(@UserID<>'')
	begin
		set @UserID=''
		select @UserID = UserID,@ClientID=ClientID,@AgentID=AgentID,@RoleID=RoleID from Users 
		where LoginName=@LoginName and LoginPWD=@LoginPWD and  Status<>9
		if(@UserID<>'')
		begin
			--会员信息
			select * from Users where UserID=@UserID

			--权限信息
			select m.* from Menu m left join RolePermission r on r.MenuCode=m.MenuCode 
			where (RoleID=@RoleID or IsLimit=0 )

			set @Result=1
		end
		else
			set @Result=4

	end
	else
		set @Result=3

end
else
set @Result=2

 

