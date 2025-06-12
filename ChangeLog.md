# 0.11.1

- Generalise the return type of Exckit.ResultFacade.tryFallible from
  ```Result<'a, System.Exception>``` to ```Result<'a, 'b>```
- Provide a convenient means to categorise exceptions, assuming the standard fatal exception
  definition.

# 0.11.0

- Extend and refine the interface for structured handling of exceptions.
