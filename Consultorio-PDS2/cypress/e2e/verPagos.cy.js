describe('Vista de Pagos del Doctor', () => {
    beforeEach(() => {
        // Login y navegar a la vista de pagos
        cy.visit('/LoginRegistro/Login')
        cy.get('input[name="idUsuario"]').type('1')
        cy.get('input[name="contrasena"]').type('1234abcd')
        cy.contains('button', 'Ingresar').click()
        cy.visit('/Pagos/PagosDelDoctor')
    })

    it('debería mostrar la tabla con la estructura correcta cuando hay pagos', () => {
        cy.get('body').then(($body) => {
            if ($body.find('table tbody tr').length > 0) {
                // Caso: Hay pagos - verificar estructura de la tabla
                cy.get('table').should('be.visible')
                cy.get('table').should('have.class', 'table-striped')

                // Verificar headers
                cy.get('table thead').should('have.class', 'table-dark')
                cy.get('table thead th').should('have.length', 4)
                cy.get('table thead th').eq(0).should('contain', 'Paciente')
                cy.get('table thead th').eq(1).should('contain', 'Monto')
                cy.get('table thead th').eq(2).should('contain', 'Método de Pago')
                cy.get('table thead th').eq(3).should('contain', 'Fecha de Pago')

                // Verificar que hay al menos un pago
                cy.get('table tbody tr').should('have.length.at.least', 1)
            }
        })
    })

    it('debería mostrar datos válidos en cada fila de pagos', () => {
        cy.get('body').then(($body) => {
            if ($body.find('table tbody tr').length > 0) {
                // Verificar todas las filas de la tabla
                cy.get('table tbody tr').each(($row) => {
                    cy.wrap($row).within(() => {
                        // Paciente debe tener al menos un nombre (no vacío)
                        cy.get('td').eq(0).invoke('text').then((text) => {
                            expect(text.trim()).to.not.be.empty
                        })

                        // Monto debe tener formato de moneda $X.XX
                        cy.get('td').eq(1).invoke('text').should('match', /^\$\d{1,3}(?:,\d{3})*\.\d{2}$/)

                        // Método de pago no debe estar vacío
                        cy.get('td').eq(2).invoke('text').then((text) => {
                            expect(text.trim()).to.not.be.empty
                        })

                        // Fecha debe tener formato dd/MM/yyyy (día/mes/año)
                        cy.get('td').eq(3).invoke('text').should('match', /^\d{2}\/\d{2}\/\d{4}$/)

                        // Validación adicional de fecha: día (01-31), mes (01-12), año (2023-2025+)
                        cy.get('td').eq(3).invoke('text').then((fecha) => {
                            const [dia, mes, año] = fecha.split('/')
                            expect(parseInt(dia)).to.be.at.least(1).and.at.most(31)
                            expect(parseInt(mes)).to.be.at.least(1).and.at.most(12)
                            expect(parseInt(año)).to.be.at.least(2023) // Año actual o futuro
                        })
                    })
                })
            }
        })
    })

    it('debería mostrar el resumen de totales cuando hay pagos', () => {
        cy.get('body').then(($body) => {
            if ($body.find('table tbody tr').length > 0) {
                const rowCount = $body.find('table tbody tr').length

                cy.get('.alert-info').should('be.visible')
                cy.get('.alert-info').should('contain', `Total de pagos: ${rowCount}`)
                cy.get('.alert-info').should('contain', 'Monto total:')
                cy.get('.alert-info').invoke('text').should('match', /Monto total:\s*\$\d+/)
            }
        })
    })

})