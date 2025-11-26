describe('Registrar Paciente', () => {
    beforeEach(() => {
        // Login primero
        cy.visit('/LoginRegistro/Login')
        cy.get('input[name="idUsuario"]').type('1')
        cy.get('input[name="contrasena"]').type('1234abcd')
        cy.contains('button', 'Ingresar').click()

        // Navegar a la vista
        cy.visit('/Doctor/RegistrarPaciente')
    })

    it('debería cargar correctamente el formulario', () => {
        cy.contains('h2', 'Registrar Nuevo Paciente').should('be.visible')

        cy.get('input[name="Nombre"]').should('exist')
        cy.get('input[name="Apellido"]').should('exist')
        cy.get('input[name="FechaNacimiento"]').should('exist')
        cy.get('input[name="Telefono"]').should('exist')
        cy.get('input[name="EMail"]').should('exist')
        cy.get('input[name="Direccion"]').should('exist')

        cy.contains('button', 'Registrar Paciente').should('be.visible')
    })

    it('debería registrar un paciente correctamente', () => {
        const random = Math.floor(Math.random() * 10000)

        cy.get('input[name="Nombre"]').type('Sebastian')
        cy.get('input[name="Apellido"]').type('PerezTest' + random)
        cy.get('input[name="FechaNacimiento"]').type('1999-04-15')
        cy.get('input[name="Telefono"]').type('6621234567')
        cy.get('input[name="EMail"]').type(`sebastian${random}@gmail.com`)
        cy.get('input[name="Direccion"]').type('Av. Universidad 555')

        cy.contains('button', 'Registrar Paciente').click()

        // Validar mensaje de éxito
        cy.get('.alert-success')
            .should('be.visible')
            .and('contain', 'Paciente registrado correctamente')
    })

    it('debería mostrar validaciones cuando el formulario está vacío', () => {
        cy.contains('button', 'Registrar Paciente').click()

        cy.get('.text-danger').each(($err) => {
            expect($err.text().trim().length).to.be.at.least(1)
        })
    })
})
