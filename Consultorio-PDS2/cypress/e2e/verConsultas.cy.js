describe('Página de Consultas del Doctor', () => {
    beforeEach(() => {
        // Primero visitar la página protegida (redirigirá al login)
        cy.visit('/Doctor/VerConsultas')

        // Completar el formulario de login
        cy.get('input[name="idUsuario"]').type('1')
        cy.get('input[name="contrasena"]').type('1234abcd')
        cy.contains('button', 'Ingresar').click()

        // Después del login, redirigir manualmente a la vista de consultas
        cy.visit('/Doctor/VerConsultas')
    })

   

        it('debería cargar pacientes en el select', () => {
            // Verificar que el select tiene pacientes
            cy.get('select[name="idPaciente"] option:not([value=""])')
                .should('have.length.at.least', 1)
                .then(($options) => {
                    // Guardar los IDs de pacientes disponibles para usar después
                    const patientIds = $options.map((i, opt) => opt.value).get()
                    cy.wrap(patientIds).as('patientIds')
                })
        })

        it('debería filtrar consultas por paciente seleccionado', function () {
            // Usar el primer paciente disponible (excluyendo la opción vacía)
            cy.get('select[name="idPaciente"] option:not([value=""])')
                .first()
                .then(($option) => {
                    const patientId = $option.val()
                    const patientName = $option.text()

                    cy.log(`Seleccionando paciente: ${patientName} (ID: ${patientId})`)

                    // Seleccionar el paciente y filtrar
                    cy.get('select[name="idPaciente"]').select(patientId)
                    cy.contains('button','Filtrar Consultas').click()

                    // Verificar que la URL incluye el parámetro del paciente
                    cy.url().should('include', `idPaciente=${patientId}`)

                    // Verificar que se muestra la tabla de consultas o el mensaje de no resultados
                    cy.get('body').then(($body) => {
                        if ($body.find('table tbody tr').length > 0) {
                            // Hay consultas - validar que la tabla se muestra
                            cy.get('table').should('be.visible')
                            cy.get('h4').should('contain', 'Consultas del paciente seleccionado')

                            // Validar que hay al menos una consulta
                            cy.get('table tbody tr').should('have.length.at.least', 1)

                            // Validar estructura de la tabla
                            cy.get('table thead th').should('have.length', 5)
                            cy.get('table thead th').first().should('contain', 'Fecha')
                        } else {
                            // No hay consultas - validar mensaje
                            cy.get('.alert-info')
                                .should('be.visible')
                                .and('contain', 'No se encontraron consultas para el paciente seleccionado')
                        }
                    })
                })
        })

        it('debería mantener la selección del paciente después de filtrar', () => {
            // Seleccionar un paciente
            cy.get('select[name="idPaciente"] option:not([value=""])')
                .first()
                .then(($option) => {
                    const patientId = $option.val()

                    cy.get('select[name="idPaciente"]').select(patientId)
                    cy.contains('button','Filtrar Consultas').click()

                    // Verificar que el select mantiene la selección
                    cy.get('select[name="idPaciente"]').should('have.value', patientId)
                })
        })
})