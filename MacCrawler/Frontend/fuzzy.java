import java.util.ArrayList;
import java.util.List;

public class fuzzy {

    public static int calculateLevenshteinDistance(String a, String b) {
        int[][] dp = new int[a.length() + 1][b.length() + 1];

        for (int i = 0; i <= a.length(); i++) {
            for (int j = 0; j <= b.length(); j++) {
                if (i == 0) {
                    dp[i][j] = j;
                } else if (j == 0) {
                    dp[i][j] = i;
                } else {
                    int cost = (a.charAt(i - 1) == b.charAt(j - 1)) ? 0 : 1;
                    dp[i][j] = Math.min(Math.min(dp[i - 1][j] + 1, dp[i][j - 1] + 1), dp[i - 1][j - 1] + cost);
                }
            }
        }
        return dp[a.length()][b.length()];
    }

    public static List<Result> fuzzySearch(String query, List<String> list,
            double threshold) {
        List<Result> results = new ArrayList<>();

        for (String item : list) {
            String normalizedQuery = query.toLowerCase();
            String normalizedItem = item.toLowerCase();

            int distance = calculateLevenshteinDistance(normalizedQuery, normalizedItem);
            double similarity = 1.0 - (double) distance /
                    Math.max(normalizedQuery.length(), normalizedItem.length());

            if (similarity >= threshold) {
                results.add(new Result(item, similarity));
            }
        }
        return results;
    }

    public static class Result {
        String item;
        double similarity;

        public Result(String item, double similarity) {
            this.item = item;
            this.similarity = similarity;
        }
    }

    public static void main(String[] args) {

        String query = "A Lange & Söhne";
        List<String> list = List.of("Bulgari / Bvlgari", "BVULGARI", "Bvlgari", "BVULGARI", "LANGE & SÖHNE",
                "A Lange & Söhne");

        double threshold = 0.0;
        List<Result> results = fuzzySearch(query, list, threshold);

        System.out.println("Fuzzy search results:");
        for (Result result : results) {
            System.out.println("Item: " + result.item + ", Similarity: " +
                    result.similarity);
        }

    }
}
