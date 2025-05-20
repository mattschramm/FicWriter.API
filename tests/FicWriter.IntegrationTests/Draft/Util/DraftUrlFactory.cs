namespace FicWriter.IntegrationTests.Draft.Util;

public static class DraftUrlFactory
{
    public static string GetDraftUrl(string workId) => $"/works/{workId}/drafts";
}
