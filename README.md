# Functional Principles in ASP.NET Core Web APIs

This repository contains an ASP.NET Core Web API sample that applies fundamental principles that lie at the foundation of functional programming, giving you insight into how your REST API will behave. 

Applying functional programming principles can give you the ability to create robust applications that quickly adapt to ever-changing market needs. I strongly recommend you to check this course: [Applying Functional Principles in C#](https://app.pluralsight.com/library/courses/csharp-applying-functional-principles/table-of-contents).


## About the sample

The **Customer Management** sample was adapted from [Vladimir Khorikov's](https://github.com/vkhorikov/FuntionalPrinciplesCsharp) repository in order to provide an ASP.NET Core version and some refactorings to adhere more clean code principles.

Some concepts used in this sample:

- [Command-query Separation principle (CQS)](https://en.wikipedia.org/wiki/Command%E2%80%93query_separation): Functions that change state (side effect) should not return values and functions that return values should not change state.
- [Fail-Fast Principle](https://enterprisecraftsmanship.com/2015/09/15/fail-fast-principle/): Stopping the current operation as soon as any unexpected error occurs.
- [Don't Repeat Yourself Principle (DRY)](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself): Every piece of knowledge must have a single, unambiguous, authoritative representation within a system.
- [Non-nullable reference types](https://enterprisecraftsmanship.com/2015/03/13/functional-c-non-nullable-reference-types/): Type whose instances can't turn into null in any way.
- [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/): A functional approach for error handling. 
