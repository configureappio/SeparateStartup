﻿using System;
using ConfigureAppIo.Demos.SayHello.SeparateStartupClean.Infrastructure;
using ConfigureAppIo.Demos.SayHello.UnitTests.Common.InfraBaseTests;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.SeparatedStartup
{
    public class DuplicateLanguageExceptionTests : DuplicateLanguageExceptionTestBase<DuplicateLanguageException>
    {
        protected override string GetExpectedMessage(string language, int index, Type type)
        {
            return DuplicateLanguageException.GetMessage(language, index, type);
        }

        protected override DuplicateLanguageException GetDuplicateLanguageException(string language, int index, Type type)
        {
            return new DuplicateLanguageException(language, index, type);
        }

        protected override DuplicateLanguageException GetDuplicateLanguageException(string language, int index, Type type,
            Exception innerException)
        {
            return new DuplicateLanguageException(language, index, type, innerException);
        }
    }
}
