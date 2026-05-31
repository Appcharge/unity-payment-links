## [2.5.0] - 2026-05-31

### Added
- `orderResponseModel` to the `onPurchaseFailed` callback, to access full order details on failed purchases.

### Updated
- Android platform version to v1.6.0.
- iOS platform version to v1.8.0.
- Bundled Android foreground service type from `dataSync` to `shortService`.
- Error codes with clearer, more specific errors that better match the current SDK behavior. See the changes in the Troubleshooting guide.

### Removed
- Exclude Add Framework Search Paths entitlement (SPM migration; the iOS XCFramework is already included in xcode project).

## [2.4.1] - 2026-04-19

### Updated
- Item amounts will now be presented as a string instead of int.

## [2.4.0] - 2026-03-03
### Changed
- Sample Scene overhaul.

### Updated
- iOS Native SDK 1.5.0.
- Android Native SDK 1.7.0.
- Improved Android automatic post-process script.

### Added
- Added Configuration support for Checkout Foreground Service.

