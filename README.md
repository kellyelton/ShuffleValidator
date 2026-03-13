# ShuffleValidator

A benchmarking framework that scores shuffling algorithms for uniformity. Implements a pluggable `IShuffle` interface, runs each algorithm 1,000 times across deck sizes 2–200, builds a position-value frequency matrix, and produces a 0–100 uniformity score.

## How It Works

1. Discovers all `IShuffle` implementations via reflection at startup
2. For each implementation, iterates deck sizes from 2 to 200 cards
3. Runs 1,000 shuffles per deck size, recording every card's final position
4. Builds a frequency matrix: for each position, how many times each card value landed there
5. Computes deviation from the expected uniform distribution (each card should appear in each position `runCount / cardCount` times)
6. Produces a score from 0–100, where 100 = perfectly uniform

Results are ranked and displayed in the console with a progress bar.

## Adding a Shuffle Algorithm

Implement the `IShuffle` interface:

```csharp
public class MyShuffle : IShuffle
{
    public void Setup(int cardCount, int maxShuffles)
    {
        // One-time setup, called before the test run
    }

    public List<int> Shuffle(IEnumerable<int> cards)
    {
        // Return the cards in shuffled order
        var list = cards.ToList();
        // ... your shuffle logic ...
        return list;
    }
}
```

The validator automatically picks up any class implementing `IShuffle` in the assembly — no registration needed.

## Included Shuffle Algorithms

### Competition Algorithms

| Algorithm | Author | Approach | RNG |
|---|---|---|---|
| **QuantumShuffleTest** | kellyelton | Uses [QuantumList&lt;T&gt;](https://github.com/kellyelton/shuffle.net) — items "collapse" from superposition on observation | `RNGCryptoServiceProvider` |
| **CryptoRandomShuffle** | Soul1355 | Fisher-Yates with proper rejection sampling for unbiased range reduction | `RNGCryptoServiceProvider` via `CryptoRandom` |
| **GraveShuffleOne** | kellyelton | Standard Fisher-Yates | `System.Random` |
| **GraveShuffleTwo** | kellyelton | Fisher-Yates with crypto RNG wrapper (`RNGShit`) and rejection sampling | `RNGCryptoServiceProvider` |
| **SoulShuffle** | Soul1355 | Fisher-Yates with crypto RNG but uses floating-point division for range reduction (subtle bias potential) | `RNGCryptoServiceProvider` via `CryptoRNG` |
| **TBronsonShuffleTest** | kellyelton | Assigns random crypto values to each card, then quicksorts by those values; retries on collision | `RNGCryptoServiceProvider` |
| **klkitchensShufflizer** | klkitchens | LINQ `OrderBy` with `Random.Next(1, 100000)` — the classic naive approach | `System.Random` |

### Control Algorithms (excluded from scoring by default)

| Algorithm | Purpose |
|---|---|
| **ControlShuffle** | Returns cards in original order — baseline (should score 0) |
| **ControlShuffleTwo** | Cycles through permutations sequentially — deterministic control |
| **FatShuffle** | Picks a random permutation number from 0 to n!, enumerates all permutations to find it — correct but computationally absurd for large decks |

## Utility Code

- **`MathExtensionMethods`** — `Factorial()` using `BigInteger`, `Permute<T>()` for full permutation enumeration, `NextPowerOfTwo()`, `Powers()` generator
- **`RNGCryptoServiceProviderExtensionMethods`** — `Next(BigInteger max)` and `Next(BigInteger min, BigInteger max)` for crypto-grade random `BigInteger` generation
- **`QuantumList<T>`** / **`QuantumState<T>`** — embedded copy of the [shuffle.net](https://github.com/kellyelton/shuffle.net) library, with an added `UnknownItems` property and `ToList()` method not present in the standalone library

## Tech Stack

- **Framework**: .NET Framework 4.5
- **Language**: C# (Visual Studio 2012)
- **Test methodology**: Chi-squared-like uniformity analysis across position-value distributions

## Development Timeline

Development began **February 28, 2013** — approximately 10 months after the [shuffle.net](https://github.com/kellyelton/shuffle.net) library. The initial push included the core validator framework, the `IShuffle` interface, the scoring engine, and several shuffle implementations. The `QuantumList<T>` was brought in directly from shuffle.net to compete against traditional Fisher-Yates variants.

Over **16 commits across 5 months** (February 28 – August 12, 2013):

- **Feb 28**: Initial framework, scoring engine, first shuffle implementations. Early version included full n! permutation enumeration ("Removed n! cause it was just too crazy"). Second commit the same day: "Beefed the shit out of it."
- **Mar 1**: Added FatShuffle (brute-force permutation picker), refinements
- **Mar 2**: Improvements to scoring methodology
- **Mar 3**: Community contributions — Soul1355 submitted `CryptoRandomShuffle` via PR #1, bringing proper rejection sampling. kellyelton fixed FatShuffle and tuned the ignore list. Soul1355's second PR fixed an incomplete shuffle bug.
- **Jun 18**: klkitchens contributed their LINQ-based shuffle
- **Aug 12**: Final merge (klkitchens PR #3), last activity on the project

The project attracted **3 contributors** (kellyelton, Soul1355, klkitchens) and **3 pull requests** — a small but genuine open-source collaboration around shuffle algorithm quality.

## AI Code Review

*Code review performed by AI (Claude) and graded relative to the era the code was written in (.NET 4.5, 2013).*

| Criterion | Grade | Notes |
|---|---|---|
| **Completeness** | B+ | Fully functional benchmarking framework. Tests 10 algorithms across 198 deck sizes at 1,000 runs each. Produces ranked scores. Control implementations provide baselines. The `ShuffleValidator` class itself is an empty shell (constructor only), but `AnalizeIndex` does the real work. |
| **Functionality** | B | The scoring engine works and produces meaningful differentiation between algorithms. The `AnalizeIndex.GetRating()` method has a quirk: the inner loop overwrites `res` each iteration instead of accumulating, so only the last card value's deviation per position contributes to the score. Despite this, the overall ranking still separates good shuffles from bad ones because the deviation pattern is consistent. |
| **Patterns & Practices** | B- | Good use of reflection for auto-discovery of `IShuffle` implementations — plug in a class and it's automatically tested. The `IShuffle` interface is clean and minimal. However, the `AnalizeResults` method name has a persistent typo ("Analize"). Some implementations embed their RNG wrapper classes inline rather than sharing. |
| **Code Quality** | B | Entertaining variable names (`asses` for assemblies, `RNGShit` for the crypto wrapper, `anal` for the analyzer). Comments are sparse but the code is mostly self-explanatory. The progress bar implementation is a nice touch for long-running benchmarks. CryptoRandomShuffle (contributed by Soul1355) has noticeably better formatting and structure than the rest. |
| **Architecture** | B+ | Clean plugin architecture via `IShuffle` interface + reflection. Separation of concerns: `Program` handles orchestration and display, `AnalizeIndex` handles statistical analysis, each shuffle is self-contained. The ignore list mechanism allows selectively excluding algorithms. |
| **Ambition** | A- | Building a shuffle algorithm benchmarking framework with statistical uniformity analysis, a plugin architecture, and controls is a sophisticated project. Testing multiple RNG strategies (System.Random vs crypto, modular vs rejection sampling vs floating-point) shows deep engagement with the problem. The FatShuffle (enumerate all n! permutations) is an audacious "what if" experiment. |
| **Time Investment** | A- | The core framework was built in a single day (Feb 28). Multiple shuffle implementations, the scoring engine, BigInteger factorial/permutation utilities, and a console progress bar — all in one session. Continued iteration over the following days with bug fixes and community integration. |
| **Idea Timing** | B+ | See section below. |

**Overall: B+** — A well-conceived benchmarking framework that successfully compares shuffle algorithms across multiple dimensions (algorithm design, RNG quality, range-reduction technique). The plugin architecture attracted community contributions. The scoring methodology has a minor accumulation bug but still produces meaningful rankings. The ambition of systematically validating shuffle quality — including controls — shows rigorous experimental thinking.

## Idea Timing

Shuffle algorithm quality analysis was a topic of growing interest in 2012–2013:

| Date | Event |
|------|-------|
| 1995 | Marsaglia publishes the Diehard Battery of Tests for RNG quality |
| 1999 | NIST SP 800-22 provides standardized crypto-RNG test suite |
| 1999 | [ASF poker exploit](https://www.datamation.com/applications/how-to-cheat-at-online-poker-a-study-in-software-security/) demonstrates real-world consequences of weak shuffle algorithms |
| 2007 | Jeff Atwood's ["The Danger of Naivete"](https://blog.codinghorror.com/the-danger-of-naivete/) visualizes naive shuffle bias — hugely influential |
| 2007 | TestU01 released — comprehensive RNG test suite (Small Crush, Crush, Big Crush) |
| 2012 Jan | Bostock publishes [shuffle comparison heatmaps](https://bost.ocks.org/mike/shuffle/compare.html) — matrix visualization of shuffle bias becomes the standard approach |
| **2012 May** | **[shuffle.net](https://github.com/kellyelton/shuffle.net) created — the QuantumList that would later be benchmarked** |
| **2013 Feb–Aug** | **ShuffleValidator — pluggable framework for scoring shuffle algorithm uniformity** |
| 2014 Dec | ["Card shuffling algorithms, good and bad"](https://possiblywrong.wordpress.com/2014/12/01/card-shuffling-algorithms-good-and-bad/) — mathematical analysis blog post |
| 2022 | Academic work on "Uniformity Testing in the Shuffle Model" (SOSA 2022) |

ShuffleValidator's approach — running many trials, building frequency matrices, and computing deviation scores — is a simplified chi-squared uniformity test applied specifically to permutation distributions. While Bostock visualized bias (2012), ShuffleValidator quantifies it as a single numeric score and provides a pluggable interface for comparing arbitrary implementations. The inclusion of control shuffles (no-op and sequential permutation) demonstrates sound experimental methodology.

The project's real value is as a companion to shuffle.net: it asks the question "does the QuantumList shuffle actually produce uniform distributions?" and provides the tooling to answer it rigorously.
