// https://docs.cypress.io/api/introduction/api.html

describe('Test navigation to inbox', () => {
  it('Visits the MessageInboxView url from the Home view', () => {
    // Arrange
    cy.visit('http://localhost:8080/home')

    // Act
    cy.contains('Inbox').click()

    // Assert
    cy.url().should('include', '/inbox/messages')
    cy.contains('Home')
    cy.contains('About')
    cy.contains('Inbox')
    cy.contains('Search')
    cy.get('[data-cy=navpane]').contains('Messages')
    cy.get('[data-cy=navpane]').contains('Invitations')
    cy.get('[data-cy=navpane]').contains('Sent')
  })
})

describe('Test inbox functionality', () => {
  // Arrange
  const url = 'http://localhost:8080/inbox/messages'
  const testUser = 'Troy Barnes'
  beforeEach(() => {
    cy.visit(url)
  })

  it('Retrieves messages from the Messages inbox', () => {
    // Act
    cy.get('[data-cy=navpane]').contains('Messages').click()

    // Assert
    cy.url().should('equal', url)
  })
  
  it('Retrieves messages from the Invitations inbox', () => {
    // Act
    cy.get('[data-cy=navpane]').contains('Invitations').click()

    // Assert
    cy.url().should('include', '/inbox/invitations')
  })
  
  it('Retrieves messages from the Sent inbox', () => {
    // Act
    cy.get('[data-cy=navpane]').contains('Sent').click()

    // Assert
    cy.url().should('include', '/inbox/sent')
  })

  it('Opens a message from an inbox', () => {
    // Act
    cy.get('[data-cy=messagelist]').click()
    cy.contains(testUser).click()

    // Assert
    cy.url().should('include', '/inbox/message/#')
  })
})
