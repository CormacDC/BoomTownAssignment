# BoomTownAssignment

This repo contains the code for Cormac Conahan's submission for BoomTown's interview assignment.

## Version Info and Details

This program is a console application that was built with .NET SDK version 6.0.101 on a Windows 10 Operating System

## Running

In a console or terminal, navigate to the directory where this repo is located on your machine and simply enter `dotnet run`.

## The Assignment

Using the GitHub API and an object-oriented language of choice (preferably C#), pull top-level details for the BoomTownROI organization at: https://api.github.com/orgs/boomtownroi

From the top-level organization details result object, complete the following:

1. Output Data:

- Follow all urls containing "api.github.com/orgs/BoomTownROI" in the path, and for responses with a 200 status code, retrieve and display all 'id' keys/values in the response objects. For all non-200 status codes, give some indication of the failed request. HINT: Devise a way for the end user to make sense of the id values, related to the original resource route used to retrieve the data.

2. Perform Verifications:

- On the top-level BoomTownROI organization details object, verify that the 'updated_at' value is later than the 'created_at' date.

- On the top-level details object, compare the 'public_repos' count against the repositories array returned from following the 'repos_url', verifying that the counts match. HINT: The public repositories resource only returns a default limit of 30 repo objects per request.
