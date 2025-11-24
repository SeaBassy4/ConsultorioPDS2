const { defineConfig } = require('cypress')

module.exports = defineConfig({
    e2e: {
        baseUrl: 'https://localhost:7081', // URL de tu app ASP.NET
        setupNodeEvents(on, config) {
            // implement node event listeners here
        },
    },

    // Configuración adicional
    viewportWidth: 1280,
    viewportHeight: 720,
    defaultCommandTimeout: 10000, // 10 segundos
})