# Slot Machine Game

A fun, interactive slot machine game built in Unity 6 with smooth animations, satisfying visual effects, and a hidden debug menu for testing.

## Overview

This is a classic 3-reel slot machine experience with a modern twist. The game features polished animations, dynamic audio feedback, and a clever win system that keeps you coming back. It's perfect for learning how game mechanics work or just having some fun spinning reels.

## Features

- **3-Reel Spinning Action** - Smooth spin animations with realistic deceleration effects
- **Dynamic Win System** - Multiple symbol combinations with payout tiers
- **Visual Effects** - Symbol glow animations, bounce effects, and coin rain for big wins
- **Audio Feedback** - Satisfying spin loops, win fanfare, and lever sounds
- **Camera Shake** - Impact feedback based on win size (normal, big, or mega)
- **Hidden Debug Menu** - Press `Ctrl+D` to access testing tools
- **Responsive UI** - Clean, readable balance and win displays
- **Anticipation System** - Reels speed up when first two symbols match (casino psychology!)

## How to Play

1. **Start the Game** - You begin with a balance of 1000 coins and a default bet of 10
2. **Pull the Lever** - Click the lever on the right side of the machine
3. **Watch the Reels** - The three reels will spin with realistic deceleration
4. **Check Your Win** - Matching symbols trigger payouts and animations
5. **Repeat** - Keep spinning until you've had enough

The game automatically checks for wins after the reels stop. If you get a near-miss (2 of 3 symbols matching), you'll feel a subtle camera shake to remind you how close you were.

## Payout Structure

### Guaranteed Wins
- **3x Seven** = 25x your bet (JACKPOT!)
- **3x Bonus** = 30x your bet (MEGA WIN!)
- **2x Bonus** = 5x your bet

### Symbol Payouts (All Three Match)
| Symbol | Multiplier |
|--------|-----------|
| Apple | 6x |
| Bar | 10x |
| Cherry | 5x |
| Coin | 15x |
| Crown | 20x |
| Watermelon | 8x |
| Wild | 12x |

**Wild Card:** The Wild symbol acts as a substitute for any other symbol, making it easier to hit matches.

## Win Tiers & Effects

Different win sizes trigger different visual experiences:

- **Normal Win (under 10x bet)** - Symbols glow and bounce gently, 20 coins rain down
- **Big Win (10-20x bet)** - More intense bouncing, 20 coins rain with wider spread
- **Mega Win (20x+ bet)** - All effects maxed out, 40 coins rain, camera shakes dramatically

## Debug Menu (Testing)

Press **Ctrl+D** to open the hidden debug menu. This gives you access to:

- **Simulate Jackpot** - Instantly land 3x Seven
- **Simulate Mega Win** - Instantly land 3x Crown (30x payout)
- **Simulate Big Win** - Instantly land 3x Coin
- **Simulate Near-Miss** - Land 2x Seven + Apple (for testing camera shake)
- **Add Balance** - Give yourself extra coins to keep playing

Perfect for testing win animations without having to grind spins manually.

## Technical Stack

- **Engine**: Unity 6 (6000.4.4f1)
- **Rendering**: Universal Render Pipeline (URP)
- **Input**: New Input System (1.19.0)
- **UI**: TextMesh Pro + Canvas system
- **Audio**: AudioSource and AudioClip system
- **Animation**: Coroutine-based timing with WaitForSeconds

## Project Structure

```
Assets/
├── Scripts/
│   ├── GameManager.cs          # Main game controller and spin logic
│   ├── Reel.cs                 # Individual reel animation and symbol management
│   ├── WinManager.cs           # Win calculation and visual effects
│   ├── AudioManager.cs         # Centralized sound management
│   ├── HandleController.cs     # Lever animation and interaction
│   ├── CameraShake.cs          # Screen vibration effects
│   └── DebugManager.cs         # Hidden debug menu system
├── Sprites/                    # Symbol and UI sprite assets
├── Animations/                 # Animation clips
├── Scenes/                     # Game scenes
└── UI/                         # Canvas and UI prefabs
```

## How It Works

### The Spin Sequence

1. Player pulls the handle → lever animates and plays sound
2. Reels begin spinning with random symbols, camera shakes slightly
3. Reel 1 stops with smooth deceleration
4. Brief pause (anticipation)
5. Reel 2 stops
6. If first two reels match, Reel 3 speeds up (casino psychology!)
7. Reel 3 stops with final deceleration
8. Win check happens automatically

### Win Detection

The game checks three scenarios:

1. **3x Seven** → 25x bet (Jackpot)
2. **3x Bonus** → 30x bet, or 2x Bonus → 5x bet
3. **Any other 3-match** → Symbol payout (with Wild substitution)
4. **2 of 3 match** → Near-miss (camera shake, no payout)

### Animation System

All animations run on coroutines with frame-perfect timing:

- **Spin Deceleration** - Cubic ease-out curve over 8 steps for smooth stop
- **Symbol Glow** - PingPong between original color and yellow for 1 second
- **Bounce Effect** - Sine wave with 3 bounces, scales up to 1.25x
- **Coin Rain** - 20-40 coins spawn with random horizontal drift and fade-out
- **Camera Shake** - Random jitter from 0.05 to 0.3 magnitude depending on win size

## Code Philosophy

The code is written to be readable and maintainable. Key decisions:

- **Singleton Pattern** - AudioManager and DebugManager use statics for easy access
- **Enum-Based State** - SymbolType and WinTier keep logic clear and type-safe
- **Coroutine Timing** - Predictable, frame-independent animations
- **Null Safety** - All external references checked before use, graceful degradation
- **Component-Based** - UI, audio, and animation systems are independent and reusable

## Known Behaviors

- **Anticipation Boost** - When the first two reels match, the third reel spins faster. This is intentional and makes close calls more dramatic.
- **Coin Destruction** - Coins automatically clean themselves up after 2.5 seconds to prevent memory buildup.
- **Near-Miss Detection** - Any 2-of-3 match triggers subtle camera shake. This is intentional for player psychology.
- **Balance Display** - Updates in real-time after every spin and win.

## Building & Running

1. Open the project in **Unity 6 (6000.4.4f1)** or later
2. Make sure you have the **New Input System** package installed
3. Open the main scene from `Assets/Scenes/`
4. Press Play or build to target platform

## Future Ideas

Some potential improvements (if you wanted to extend this):

- Persistent leaderboards with best wins
- Difficulty settings affecting payout rates
- Additional symbol themes or seasonal variations
- Sound volume controls
- Multiplier bonuses every N spins
- Animation speed settings
- Mobile touch support

## Performance Notes

The game is optimized for smooth 60fps gameplay:

- Reels use simple sprite swapping instead of mesh deformation
- Coin rain cleans up automatically to prevent memory leaks
- Audio uses single-source playback with one-shots
- No physics bodies needed (all positioned via RectTransform)

## Credits

Built with love using Unity's powerful 2D system. Special thanks to the URP team for making rendering smooth and the New Input System for being way better than the legacy input.

---

**Have fun spinning! And remember, in a slot machine, the house always wins... unless you hit that jackpot! 🎰**
