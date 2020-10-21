Feature: Train Reservation
	As a French start-up, TrainTrain Company is aimed at helping passengers 
	to reserve seats in national trains smoothly.

Scenario: For a train overall, no more than 70% of seats may be reserved in advance
	Given one coach empty made of 10 seats
	When 3 seats are requested
	Then the reservation should be assigned these seats "1A, 2A, 3A"

Scenario: Not reserve seats when it exceeds train max capacity 70%
	Given 1 coaches of 10 seats and 6 already reserved
	When 3 seats are requested
	Then the reservation should be failed

Scenario: Each reservation must be booked in the same coach
	Given 2 coaches of 10 seats and 9 seats already reserved in the coach 1
	When 2 seats are requested
	Then the reservation should be assigned these seats "1B, 2B"