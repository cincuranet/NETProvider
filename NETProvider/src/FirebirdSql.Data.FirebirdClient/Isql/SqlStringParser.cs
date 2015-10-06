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
using System.Globalization;
using System.Linq;
using System.Text;

namespace FirebirdSql.Data.Isql
{
	class SqlStringParser
	{
		string _source;
		int _sourceLength;
		string[] _tokens;
		int _currentIndex;
		string _result;
		string _resultClean;

		public string Result => _result;

		public string ResultClean => _resultClean;

		public string[] Tokens
		{
			get { return _tokens; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				foreach (var item in value)
				{
					if (value == null)
						throw new ArgumentNullException();
					if (string.IsNullOrEmpty(item))
						throw new ArgumentException();
				}
				_tokens = value;
			}
		}

		public SqlStringParser(string targetString)
		{
			_tokens = new[] { " " };
			_source = targetString;
			_sourceLength = targetString.Length;
		}

		public int ParseNext()
		{
			if (_currentIndex >= _sourceLength)
			{
				return -1;
			}

			var rawResult = new StringBuilder();
			var index = _currentIndex;
			while (index < _sourceLength)
			{
                if (GetChar(index) == '\'')
				{
					rawResult.Append(GetChar(index));
					index++;
					rawResult.Append(ProcessLiteral(ref index));
					rawResult.Append(GetChar(index));
					index++;
				}
				else if (GetChar(index) == '-' && GetNextChar(index) == '-')
				{
					index++;
					ProcessSinglelineComment(ref index);
					index++;
				}
				else if (GetChar(index) == '/' && GetNextChar(index) == '*')
				{
					index++;
					ProcessMultilineComment(ref index);
					index++;
				}
				else
				{
					foreach (var token in Tokens)
					{
						if (string.Compare(_source, index, token, 0, token.Length, false, CultureInfo.CurrentUICulture) == 0)
						{
							index += token.Length;
							var matchedToken = token;
							_result = _source.Substring(_currentIndex, index - _currentIndex - token.Length);
							_resultClean = rawResult.ToString();
							return _currentIndex = index;
						}
					}
					if (!(rawResult.Length == 0 && char.IsWhiteSpace(GetChar(index))))
					{
						rawResult.Append(GetChar(index));
					}
					index++;
				}
			}

			if (index > _sourceLength)
			{
				_result = _source.Substring(_currentIndex);
				_resultClean = rawResult.ToString();
				return _currentIndex = _sourceLength;
			}
			else
			{
				_result = _source.Substring(_currentIndex, index - _currentIndex);
				_resultClean = rawResult.ToString();
				return _currentIndex = index;
			}
		}

		string ProcessLiteral(ref int index)
		{
			var sb = new StringBuilder();
			while (index < _sourceLength)
			{
				if (GetChar(index) == '\'')
				{
					if (GetNextChar(index) == '\'')
					{
						sb.Append(GetChar(index));
						index++;
					}
					else
					{
						break;
					}
				}
				sb.Append(GetChar(index));
				index++;
			}
			return sb.ToString();
		}

		void ProcessMultilineComment(ref int index)
		{
			while (index < _sourceLength)
			{
				if (GetChar(index) == '*' && GetNextChar(index) == '/')
				{
					index++;
					break;
				}
				index++;
			}
		}

		void ProcessSinglelineComment(ref int index)
		{
			while (index < _sourceLength)
			{
				if (GetChar(index) == '\n')
				{
					break;
				}
				if (GetChar(index) == '\r')
				{
					if (GetNextChar(index) == '\n')
					{
						index++;
					}
					break;
				}
				index++;
			}
		}

		char GetChar(int index)
		{
			return _source[index];
		}

		char? GetNextChar(int index)
		{
			return index + 1 < _sourceLength
				? _source[index + 1]
				: (char?)null;
		}
	}
}
