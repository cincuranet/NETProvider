﻿/*
 *  Firebird ADO.NET Data provider for .NET and Mono
 *
 *     The contents of this file are subject to the Initial
 *     Developer's Public License Version 1.0 (the "License");
 *     you may not use this file except in compliance with the
 *     License. You may obtain a copy of the License at
 *     http://www.firebirdsql.org/index.php?op=doc&id=idpl
 *
 *     Software distributed under the License is distributed on
 *     an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 *     express or implied.  See the License for the specific
 *     language governing rights and limitations under the License.
 *
 *  Copyright (c) 2003, 2005 Abel Eduardo Pereira
 *  All Rights Reserved.
 *
 * Contributors:
 *   Jiri Cincura (jiri@cincura.net)
 */

using System;
using FirebirdSql.Data.FirebirdClient;

namespace FirebirdSql.Data.Isql
{
	public class CommandExecutingEventArgs : EventArgs
	{
		public FbCommand SqlCommand { get; private set; }
		public SqlStatementType StatementType { get; private set; }

		public CommandExecutingEventArgs(FbCommand sqlCommand, SqlStatementType statementType)
		{
			SqlCommand = sqlCommand;
			StatementType = statementType;
		}
	}
}
