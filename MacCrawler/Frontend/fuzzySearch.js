function calculateLevenshteinDistance(str1, str2) {
    const dp = Array.from({ length: str1.length + 1 }, (_, i) => Array(str2.length + 1).fill(i));
    for (let i = 0; i <= str1.length; i++) {
        for (let j = 0; j <= str2.length; j++) {
            if (i === 0) {
                dp[i][j] = j;
            } else if (j === 0) {
                dp[i][j] = i;
            } else {
                const cost = str1.charAt(i - 1) === str2.charAt(j - 1) ? 0 : 1;
                dp[i][j] = Math.min(
                    Math.min(dp[i - 1][j] + 1, dp[i][j - 1] + 1),
                    dp[i - 1][j - 1] + cost
                );
            }
        }
    }
    return dp[str1.length][str2.length];
}
export function fuzzySearch(query, inputString, threshold) {
    var normalizedQuery = query.toLowerCase();
    var inputQuery = inputString.toLowerCase();
    var distance = calculateLevenshteinDistance(normalizedQuery, inputQuery);
    var similarity = 1.0 - distance / Math.max(normalizedQuery.length, inputQuery.length)
    if (similarity >= threshold) {
        return true;
    }
    return false;
}


