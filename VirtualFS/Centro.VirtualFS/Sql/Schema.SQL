/*USE [FileExplorer]
GO*/

/* drop statements */
ALTER TABLE [dbo].[AccessControlLists] DROP CONSTRAINT [FK_AccessControlLists_Directories]
GO
ALTER TABLE [dbo].[ACLMembers] DROP CONSTRAINT [FK_ACLMembers_AccessControlLists]
GO
ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_Files_Directories]
GO
ALTER TABLE [dbo].[FileVersions] DROP CONSTRAINT [FK_FileVersions_FileBinaries]
GO
ALTER TABLE [dbo].[FileVersions] DROP CONSTRAINT [FK_FileVersions_Files]
GO
ALTER TABLE [dbo].[RootDirectories] DROP CONSTRAINT [FK_RootDirectories_Directories]
GO
DROP TABLE [dbo].[RootDirectories]
GO
DROP TABLE [dbo].[FileVersions]
GO
DROP TABLE [dbo].[ACLMembers]
GO
DROP TABLE [dbo].[Directories]
GO
DROP TABLE [dbo].[Files]
GO
DROP TABLE [dbo].[FileBinaries]
GO
DROP TABLE [dbo].[AccessControlLists]
GO


/****** Object:  Table [dbo].[Directories]    Script Date: 12/03/2007 11:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Directories](
	[directoryid] [bigint] IDENTITY(1,1) NOT NULL,
	[directoryname] [varchar](255) NOT NULL,
	[parentdirectoryid] [bigint] NOT NULL,
	[directorydeleted] [bit] NOT NULL CONSTRAINT [DF_Directories_directorydeleted]  DEFAULT ((0)),
	[rootdirectory] [bit] NOT NULL CONSTRAINT [DF_Directories_rootdirectory]  DEFAULT ((0)),
 CONSTRAINT [PK_Directories] PRIMARY KEY CLUSTERED 
(
	[directoryid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO



/****** Object:  Table [dbo].[FileBinaries]    Script Date: 12/03/2007 11:03:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileBinaries](
	[filebinaryid] [bigint] IDENTITY(1,1) NOT NULL,
	[filebinarydata] [image] NOT NULL,
	[filesize] [bigint] NOT NULL,
 CONSTRAINT [PK_FileBinaries] PRIMARY KEY CLUSTERED 
(
	[filebinaryid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




/****** Object:  Table [dbo].[AccessControlLists]    Script Date: 12/03/2007 11:03:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessControlLists](
	[aclid] [bigint] IDENTITY(1,1) NOT NULL,
	[directoryid] [bigint] NOT NULL,
	[aclreadall] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_aclreadall]  DEFAULT ((1)),
	[aclchangeall] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_aclchangeall]  DEFAULT ((0)),
	[acladminall] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_acladminall]  DEFAULT ((0)),
	[inherit] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_inherit]  DEFAULT ((1)),
 CONSTRAINT [PK_AccessControlLists] PRIMARY KEY CLUSTERED 
(
	[aclid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




/****** Object:  Table [dbo].[Files]    Script Date: 12/03/2007 11:03:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Files](
	[fileid] [bigint] IDENTITY(1,1) NOT NULL,
	[filename] [varchar](255) NOT NULL,
	[directoryid] [bigint] NOT NULL,
	[filecreationdate] [datetime] NOT NULL,
	[filedeleted] [bit] NOT NULL CONSTRAINT [DF_Files_filedeleted]  DEFAULT ((0)),
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[fileid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO




/****** Object:  Table [dbo].[FileVersions]    Script Date: 12/03/2007 11:03:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileVersions](
	[fileversionid] [bigint] IDENTITY(1,1) NOT NULL,
	[fileid] [bigint] NOT NULL,
	[versionnumber] [int] NOT NULL,
	[filebinaryid] [bigint] NOT NULL,
	[filemodifieddate] [datetime] NOT NULL,
	[latestversion] [bit] NOT NULL CONSTRAINT [DF_FileVersions_latestversion]  DEFAULT ((1)),
 CONSTRAINT [PK_FileVersions] PRIMARY KEY CLUSTERED 
(
	[fileversionid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




/****** Object:  Table [dbo].[ACLMembers]    Script Date: 12/03/2007 11:03:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ACLMembers](
	[aclmemberid] [bigint] IDENTITY(1,1) NOT NULL,
	[aclid] [bigint] NOT NULL,
	[membername] [varchar](100) NOT NULL,
	[aclread] [bit] NOT NULL CONSTRAINT [DF_ACLMembers_aclread]  DEFAULT ((0)),
	[aclchange] [bit] NOT NULL CONSTRAINT [DF_ACLMembers_aclchange]  DEFAULT ((0)),
	[acladmin] [bit] NOT NULL CONSTRAINT [DF_ACLMembers_acladmin]  DEFAULT ((0)),
 CONSTRAINT [PK_ACLMembers] PRIMARY KEY CLUSTERED 
(
	[aclmemberid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO




/****** Object:  ForeignKey [FK_AccessControlLists_Directories]    Script Date: 12/03/2007 11:03:18 ******/
ALTER TABLE [dbo].[AccessControlLists]  WITH CHECK ADD  CONSTRAINT [FK_AccessControlLists_Directories] FOREIGN KEY([directoryid])
REFERENCES [dbo].[Directories] ([directoryid])
GO
ALTER TABLE [dbo].[AccessControlLists] CHECK CONSTRAINT [FK_AccessControlLists_Directories]
GO

/****** Object:  ForeignKey [FK_ACLMembers_AccessControlLists]    Script Date: 12/03/2007 11:03:22 ******/
ALTER TABLE [dbo].[ACLMembers]  WITH CHECK ADD  CONSTRAINT [FK_ACLMembers_AccessControlLists] FOREIGN KEY([aclid])
REFERENCES [dbo].[AccessControlLists] ([aclid])
GO
ALTER TABLE [dbo].[ACLMembers] CHECK CONSTRAINT [FK_ACLMembers_AccessControlLists]
GO

/****** Object:  ForeignKey [FK_Files_Directories]    Script Date: 12/03/2007 11:03:30 ******/
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_Files_Directories] FOREIGN KEY([directoryid])
REFERENCES [dbo].[Directories] ([directoryid])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_Files_Directories]
GO

/****** Object:  ForeignKey [FK_FileVersions_FileBinaries]    Script Date: 12/03/2007 11:03:34 ******/
ALTER TABLE [dbo].[FileVersions]  WITH CHECK ADD  CONSTRAINT [FK_FileVersions_FileBinaries] FOREIGN KEY([filebinaryid])
REFERENCES [dbo].[FileBinaries] ([filebinaryid])
GO
ALTER TABLE [dbo].[FileVersions] CHECK CONSTRAINT [FK_FileVersions_FileBinaries]
GO

/****** Object:  ForeignKey [FK_FileVersions_Files]    Script Date: 12/03/2007 11:03:34 ******/
ALTER TABLE [dbo].[FileVersions]  WITH CHECK ADD  CONSTRAINT [FK_FileVersions_Files] FOREIGN KEY([fileid])
REFERENCES [dbo].[Files] ([fileid])
GO
ALTER TABLE [dbo].[FileVersions] CHECK CONSTRAINT [FK_FileVersions_Files]
GO