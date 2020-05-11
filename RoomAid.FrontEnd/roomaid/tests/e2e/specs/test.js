// // https://docs.cypress.io/api/introduction/api.html


describe('Search Testing', () => {
  /// Test ability of web page to query backend without filters
  it('Visit the Search Page and Search by City Name', () => {
    //Arrange
    cy.visit('https://localhost:44394')
    cy.wait(700);
    cy.contains('Search').click();
    cy.wait(700);
    cy.get('input#input-15').type('Cypress').type('{enter}')
    cy.wait(700);
    cy.get('div.v-list-item__title').click();
    cy.wait(700)
    //Act
    cy.get('span.v-btn__content').click();
    cy.wait(700)
    //Assert
    cy.url().should('include', '/search')
    cy.get('p#results').should('have.text','Households: [\n  {\n    "images": null,\n    "householdType": "Apartment",\n    "listingDescription": "Looking for 1 applicant",\n    "price": "400.0000",\n    "hostName": null\n  },\n  {\n    "images": null,\n    "householdType": "Townhouse",\n    "listingDescription": "Roomate wanted",\n    "price": "250.0000",\n    "hostName": null\n  }\n]')
  })

  /// Test ability of web page to query backend with filters
  it('Search With All Filters', () => {
    //Arrange
    cy.visit('https://localhost:44394')
    cy.wait(700);
    cy.contains('Search').click();
    cy.wait(700);
    cy.get('input#input-15').type('Cypress').type('{enter}')
    cy.wait(700);
    cy.get('div.v-list-item__title').click();
    cy.wait(700)
    cy.get('div.v-select__selections').click();
    cy.wait(700)
    cy.contains('Apartment').click();
    cy.wait(700)
    cy.get('input#input-26').type('100')
    cy.wait(700)
    cy.get('input#input-31').type('1000')
    cy.wait(700)
    //Act
    cy.get('span.v-btn__content').click();
    cy.wait(700)
    //Assert
    cy.url().should('include', '/search')
    cy.get('p#results').should('have.text','Households: [\n  {\n    "images": null,\n    "householdType": "Apartment",\n    "listingDescription": "Looking for 1 applicant",\n    "price": "400.0000",\n    "hostName": null\n  }\n]')
  })

  /// Test ability of web page to validate city input
  it('Search With invalid city', () => {
    //Arrange
    cy.visit('https://localhost:44394')
    cy.wait(700);
    cy.contains('Search').click();
    cy.wait(700);
    //Act
    cy.get('span.v-btn__content').click();
    //Assert
    cy.get('div.v-alert__content').should('have.text', ' [\n  "Please select a city from the list!"\n] ' )
  })

  /// Test ability of web page to validate price inputs
  it('Search with Invalid Price Filters', () => {
    //Arrange
    cy.visit('https://localhost:44394')
    cy.wait(700);
    cy.contains('Search').click();
    cy.wait(700);
    cy.get('input#input-15').type('Cypress');
    cy.wait(700);
    cy.get('div.v-list-item__title').click();
    cy.wait(700)
    cy.get('input#input-26').type('-1')
    cy.wait(700)
    cy.get('input#input-31').type('10001')
    cy.wait(700)
    //Act
    cy.get('span.v-btn__content').click();
    cy.wait(700)
    //Assert
    cy.get('div.v-alert__content').should('have.text', ' [\n  "The maximum price filter must be between 0 and 10000",\n  "The minimum price filter must be between 0 and 10000"\n] ')
  })

  /// Test ability of web page to adjust alerts depending on errors fixed
  it('Search With Invalid Price Filters Show Fixed Filter Alert being removed', () => {
    cy.visit('https://localhost:44394')
    cy.wait(700);
    cy.contains('Search').click();
    cy.wait(700);
    cy.get('input#input-15').type('Cypress').type('{enter}')
    cy.wait(700);
    cy.get('div.v-list-item__title').click();
    cy.wait(700)
    cy.get('input#input-26').type('-1')
    cy.wait(700)
    cy.get('input#input-31').type('10001')
    cy.wait(700)
    //Act
    cy.get('span.v-btn__content').click();
    cy.wait(700)
    cy.get('input#input-31').clear();
    cy.wait(700)
    cy.get('input#input-31').type('1000')
    cy.wait(700)
    cy.get('span.v-btn__content').click();
    //Assert
    cy.get('div.v-alert__content').should('have.text', ' [\n  "The minimum price filter must be between 0 and 10000"\n] ')
  })

  // Test Autocomplete functionality

  it('Select city from autocompleted list after a few letters' , () => {
    //Arrange
    cy.visit('https://localhost:44394')
    cy.wait(700);
    cy.contains('Search').click();
    cy.wait(700);
    cy.get('input#input-15').type('Long')
    cy.wait(1400);
    cy.contains('Long Beach').click();
    //Act
    cy.get('span.v-btn__content').click();
    cy.wait(700)
    //Arrange
    cy.get('input#input-15').should('have.value', 'Long Beach')
  })
})
