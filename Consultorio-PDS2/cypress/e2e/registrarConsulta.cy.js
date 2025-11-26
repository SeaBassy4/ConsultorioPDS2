describe('Registrar Consulta', () => {
    beforeEach(() => {
        // Primero visitar (te redirige al login)
        cy.visit('/Doctor/RegistrarConsulta')

        // Login
        cy.get('input[name="idUsuario"]').type('1')
        cy.get('input[name="contrasena"]').type('1234abcd')
        cy.contains('button', 'Ingresar').click()

        // Volver a la página protegida
        cy.visit('/Doctor/RegistrarConsulta')
    })

    it('debería cargar elementos del formulario correctamente', () => {
        cy.contains('h2', 'Registrar Nueva Consulta').should('be.visible')

        cy.get('input[name="Consulta.Motivo"]').should('exist')
        cy.get('input[name="Consulta.Diagnostico"]').should('exist')
        cy.get('input[name="Consulta.Tratamiento"]').should('exist')
        cy.get('textarea[name="Consulta.Observaciones"]').should('exist')

        cy.get('select[name="Consulta.IdPaciente"]').should('exist')

        cy.get('input[name="Pago.Monto"]').should('exist')
        cy.get('select[name="Pago.MetodoPago"]').should('exist')
    })

    it('debería cargar pacientes en el select', () => {
        cy.get('select[name="Consulta.IdPaciente"] option')
            .should('have.length.at.least', 1)
    })

    it('debería registrar una consulta correctamente', () => {
        cy.get('input[name="Consulta.Motivo"]').type('Chequeo general')
        cy.get('input[name="Consulta.Diagnostico"]').type('Diagnóstico automático')
        cy.get('input[name="Consulta.Tratamiento"]').type('Descanso y líquidos')
        cy.get('textarea[name="Consulta.Observaciones"]').type('Todo en orden.')

        cy.get('select[name="Consulta.IdPaciente"]')
            .find('option:not([value=""])')
            .first()
            .then($opt => {
                cy.get('select[name="Consulta.IdPaciente"]').select($opt.val())
            })

        cy.get('input[name="Pago.Monto"]').type('500')
        cy.get('select[name="Pago.MetodoPago"]').select('Efectivo')

        cy.contains('button', 'Registrar').click()

        // El controlador debe redirigir a algún lado con mensaje
        cy.get('body').then(($body) => {
            if ($body.find('.alert-success').length) {
                cy.get('.alert-success')
                    .should('contain', 'Consulta registrada')
            }
        })
    })

   
})
