const os = require('os');

module.exports = function (config) {
  // Determine the Chromium binary path based on the environment
  const chromiumPath = (() => {
    const platform = os.platform();
    if (platform === 'win32') {
      return 'C:\\Program Files\\Chromium\\Application\\chrome.exe'; // Windows path
    } else if (platform === 'darwin') {
      return '/Applications/Chromium.app/Contents/MacOS/Chromium'; // macOS path
    } else if (platform === 'linux') {
      return '/usr/bin/chromium-browser'; // Linux path (GitHub Actions)
    }
    return null; // Default to null if no match
  })();

  // Set the CHROME_BIN environment variable
  process.env.CHROME_BIN = chromiumPath;

  config.set({
    frameworks: ['jasmine', '@angular-devkit/build-angular'],
    plugins: [
      require('karma-jasmine'),
      require('karma-chrome-launcher'),
      require('@angular-devkit/build-angular/plugins/karma'),
    ],
    browsers: ['ChromiumHeadless'], // Use the custom launcher
    customLaunchers: {
      ChromiumHeadless: {
        base: 'ChromeHeadless',
        flags: ['--no-sandbox', '--disable-gpu'], // Required for CI environments
      },
    },
    singleRun: true, // Ensure the test runner exits after tests complete
  });
};
