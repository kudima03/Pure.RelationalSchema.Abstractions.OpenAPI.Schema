using Microsoft.OpenApi;

namespace Pure.RelationalSchema.Abstractions.OpenAPI.Schema.Tests;

public sealed record RelationalSchemaDocumentTransformerTests
{
    private static OpenApiDocument BuildDocumentWithSchema(
        string name,
        JsonSchemaType type
    )
    {
        return new OpenApiDocument
        {
            Components = new OpenApiComponents
            {
                Schemas = new Dictionary<string, IOpenApiSchema>
                {
                    [name] = new OpenApiSchema { Type = type },
                },
            },
        };
    }

    private static IOpenApiSchema Schema(OpenApiDocument document, string name)
    {
        return document.Components!.Schemas![name];
    }

    [Fact]
    public async Task IColumnIsReplacedWithObjectSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IColumn",
            JsonSchemaType.Object
        );

        await new RelationalSchemaDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "IColumn");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        Assert.Equal(JsonSchemaType.String, schema.Properties!["Name"].Type);
        Assert.Equal(JsonSchemaType.String, schema.Properties!["Type"].Type);
    }

    [Fact]
    public async Task IIndexIsReplacedWithObjectSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IIndex",
            JsonSchemaType.Object
        );

        await new RelationalSchemaDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "IIndex");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        Assert.Equal(JsonSchemaType.Boolean, schema.Properties!["IsUnique"].Type);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["Columns"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["Columns"].Items!);
    }

    [Fact]
    public async Task ITableIsReplacedWithObjectSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "ITable",
            JsonSchemaType.Object
        );

        await new RelationalSchemaDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "ITable");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        Assert.Equal(JsonSchemaType.String, schema.Properties!["Name"].Type);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["Columns"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["Columns"].Items!);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["Indexes"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["Indexes"].Items!);
    }

    [Fact]
    public async Task IForeignKeyIsReplacedWithObjectSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IForeignKey",
            JsonSchemaType.Object
        );

        await new RelationalSchemaDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "IForeignKey");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["ReferencingTable"]);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["ReferencingColumns"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(
            schema.Properties!["ReferencingColumns"].Items!
        );
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["ReferencedTable"]);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["ReferencedColumns"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(
            schema.Properties!["ReferencedColumns"].Items!
        );
    }

    [Fact]
    public async Task ISchemaIsReplacedWithObjectSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "ISchema",
            JsonSchemaType.Object
        );

        await new RelationalSchemaDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "ISchema");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        Assert.Equal(JsonSchemaType.String, schema.Properties!["Name"].Type);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["Tables"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["Tables"].Items!);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["ForeignKeys"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["ForeignKeys"].Items!);
    }

    [Fact]
    public async Task UnrelatedSchemaIsNotModified()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "MyCustomType",
            JsonSchemaType.Object
        );

        await new RelationalSchemaDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.Object, Schema(document, "MyCustomType").Type);
    }

    [Fact]
    public async Task NullComponentsDoesNotThrow()
    {
        OpenApiDocument document = new();

        await new RelationalSchemaDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );
    }
}
