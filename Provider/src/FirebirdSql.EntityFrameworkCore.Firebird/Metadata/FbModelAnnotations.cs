﻿/*
 *    The contents of this file are subject to the Initial
 *    Developer's Public License Version 1.0 (the "License");
 *    you may not use this file except in compliance with the
 *    License. You may obtain a copy of the License at
 *    https://github.com/FirebirdSQL/NETProvider/blob/master/license.txt.
 *
 *    Software distributed under the License is distributed on
 *    an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 *    express or implied. See the License for the specific
 *    language governing rights and limitations under the License.
 *
 *    All Rights Reserved.
 */

//$Authors = Jiri Cincura (jiri@cincura.net), Jean Ressouche, Rafael Almeida

using FirebirdSql.EntityFrameworkCore.Firebird.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FirebirdSql.EntityFrameworkCore.Firebird.Metadata
{
	public class FbModelAnnotations : RelationalModelAnnotations, IFbModelAnnotations
	{
		public FbModelAnnotations(IModel model)
			: base(model)
		{ }

		protected FbModelAnnotations(RelationalAnnotations annotations)
			: base(annotations)
		{ }

		public virtual FbValueGenerationStrategy? ValueGenerationStrategy
		{
			get => (FbValueGenerationStrategy?)Annotations.Metadata[FbAnnotationNames.ValueGenerationStrategy];
			set => Annotations.SetAnnotation(FbAnnotationNames.ValueGenerationStrategy, value);
		}
	}
}
