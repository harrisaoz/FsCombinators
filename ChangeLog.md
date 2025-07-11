# 0.11.4

Move the project README to the repository root to allow nuget.org and github to use it as the
package/repository README.

# 0.11.3

Document the FsCombinators project in a README.

# 0.11.2

Add required project information for publishing to nuget.org.

# 0.11.1

- Generalise the return type of Exckit.ResultFacade.tryFallible from
  ```Result<'a, System.Exception>``` to ```Result<'a, 'b>```
- Provide a convenient means to categorise exceptions, assuming the standard fatal exception
  definition.

# 0.11.0

- Extend and refine the interface for structured handling of exceptions.
