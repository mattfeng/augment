# Current issues


# Bugs that occured during development

### `The object of type 'ImageTargetBehaviour' has been destroyed but...`
This means that the template you are trying to use hasn't actually been instantiated for each
ImageTarget you wish to make from it; in order to instantiate, use the following:

```csharp
    GameObject newImageTarget = Instantiate(ImageTargetTemplate.gameObject) as GameObject;
```