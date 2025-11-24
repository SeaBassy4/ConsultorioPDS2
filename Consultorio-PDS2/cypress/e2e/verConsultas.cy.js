describe('Página de Consultas del Doctor', () => {
    beforeEach(() => {
        // Visitar la página antes de cada prueba
        cy.visit('/Doctor/VerConsultas')
    })

    it('debería cargar la página correctamente', () => {
        // Verificar elementos básicos
        cy.contains('h2', 'Consultas del Doctor').should('be.visible')
    ¿
    })

})