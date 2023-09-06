# .NET Microservices: CQRS and Event Sourcing Course Project

A Udemy course project in which we implement a simple social media back-end using CQRS and event-sourcing.
The solution contains 3 projects: a shared domain, a command API, and a query API.

Udemy course link:
https://www.udemy.com/course/net-microservices-cqrs-event-sourcing-with-kafka

Course Author: Sean Campbell (CampbellTechIO)

## Course Architecture Diagram

Architecture Diagram:
https://app.diagrams.net/#G18-djczO-0VAlckEuS0t7JSz3i90VWTbO

* Overall application architecture
* Kafka consumers diagram
    - We only implement the read-replica consumer in this course, but this illustrates the behavior of multiple consumers of one producer
    - Sequence is important!
    - Consumers track their offset by consumer group