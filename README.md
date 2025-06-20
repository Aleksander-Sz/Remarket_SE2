# Remarket_SE2
After a long descussion, here is our planning

## Technology:
# Frontend: HTML, CSS,React
# Backend: C#
We will use and SQL database to store all the necessary data. The database design will be stored as a .sql file containing the commands for creating the database. The example data will be stored in the same way. We will use C# for handling the http requests.

## Conventions:
gitflow
Scrum
All the commits shall be named according to the gitflow 
convention:


We will use two main branches: main and develop and personal branches. No commits to the main or develop branch should be made. When working on more complex parts of the project everyone is encouraged to create additional branches and then merge them back into their personal branch. Pull requests to the main branch will happen after finishing a specific stage/ feature of the project, they shall not be made daily. Pull requests from the personal branches shall be madefrequently. 

    feat: Introducing new features or significant improvements.
    fix: Bug fixes that resolve issues in your code.
    docs: Updates or additions to documentation.
    style: Cosmetic changes that don't affect code functionality (like formatting).
    refactor: Code changes that neither fix a bug nor add a feature but improve structure.
    test: Everything about testing - adding or fixing tests.
    chore: Routine tasks or updates to the build process.
    perf: Enhancements that improve performance.
    ci: Modifications related to CI/CD processes.
    build: Changes affecting the build system or external dependencies.
    revert: Undoing previous changes.

The commit message should consist of the category and, after a comma, the explanation.
We will use two main branches: main and develop and feature branches. No commits to the main branch should be made. When working on more complex parts of the project everyone is encouraged to create additional feature branches and then merge them back into the develop branch. Pull requests to the main branch will happen after finishing a specific stage/ feature of the project, they shall not be made daily.

## Roles:
# Aleksander Szydlowski: Integration
# Aleksandra Struzik: Back-end
# Esbol Erlan:Front-end
# Patryk Sukiennik:Back-end


## Scheudle
Trello


## Time Estimation
FInish 1 sprint within 2 weeks

## Backend
# This section describes, how to interact with the backend api
# Available endpoints:
    /products [GET] - returns listings from the database, arguments: category, min_price, max_price, page, limit, id, sellerId (the id is not to be used together with filters)
    /photo/{id} [GET] - returns a photo with the specific id
    /photo [POST] - adds a photo to data base, it returns the id of added photo
    /login [POST] - is used for logging in
    /account [GET] - returns the account details of the logged in user
    /categories [GET] - returns the list of all available categories
    /register [POST] - used to create the user
    /cart [GET] - returns the list of items in the cart
    /addListing [POST] - adds product to the database, arguments: title, header, paragraph, category, price
    /user/{id} [GET] - returns the id, username and photoId of the selected user
    /reviews [GET] - returns reviews from the database, arguments: userId, listingId
    /reviews/forUser/{id} - returns reviews given to the products sold by a specific user
    /changeRole/{id} [POST] - updates the role of a user
    /changeProfile/{id} [POST] - updates the user profile, attributes: NewUsername, NewDescription, NewPhotoId, NewPassword
    /orders [GET] - gets a list of the orders based on sellerId or buyerId
