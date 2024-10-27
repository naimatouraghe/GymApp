import React, { useState, useEffect } from 'react'; 
import axios from 'axios';
import { Card, Button, Form, Container, Row, Col, Alert, Modal } from 'react-bootstrap';
import { FaTimes } from 'react-icons/fa';
import './Reservations.css'; // Ensure the CSS file is imported

function Reservations() {
    const [activities, setActivities] = useState([]);
    const [reservations, setReservations] = useState([]);
    const [selectedActivityId, setSelectedActivityId] = useState('');
    const [reservationDate, setReservationDate] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [showModal, setShowModal] = useState(false);
    const [currentReservation, setCurrentReservation] = useState(null);
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

    const storedUser = JSON.parse(localStorage.getItem('user'));
    const UtilisateurId = storedUser?.id;

    const formatDateTime = (dateTimeString) => {
        const options = {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false,
        };
        const date = new Date(dateTimeString);
        return new Intl.DateTimeFormat('fr-FR', options).format(date);
    };

    useEffect(() => {
        axios.get('https://localhost:7126/api/Activite')
            .then(response => {
                setActivities(Array.isArray(response.data) ? response.data : response.data.$values || []);
            })
            .catch(() => setError('Failed to fetch activities'));
    }, []);

    useEffect(() => {
        if (!UtilisateurId) {
            setError('User not logged in');
            return;
        }

        axios.get(`https://localhost:7126/api/Reservation/${UtilisateurId}`)
        .then(response => {
            const fetchedReservations = Array.isArray(response.data.$values) ? response.data.$values : [];
            setReservations(fetchedReservations);
        })
        .catch(() => {
            setError('Failed to fetch reservations for the user');
        });
    }, [UtilisateurId]);

    const handleReservation = (e) => {
        e.preventDefault();

        if (!selectedActivityId || !reservationDate) {
            setError('Please select an activity and date.');
            return;
        }

        const selectedActivity = activities.find(activity => activity.id === selectedActivityId);

        const reservationData = {
            ActiviteId: selectedActivityId,
            UtilisateurId,
            DateHeure: reservationDate,
            ActiviteNom: selectedActivity ? selectedActivity.nom : '',
        };

        axios.post('https://localhost:7126/api/Reservation', reservationData)
            .then(() => {
                setSuccess('Reservation successfully created!');
                setError('');
                setReservationDate('');
                setSelectedActivityId('');
                axios.get(`https://localhost:7126/api/Reservation/${UtilisateurId}`)
                    .then(response => setReservations(Array.isArray(response.data) ? response.data : []))
                    .catch(() => setError('Failed to update reservations'));
            })
            .catch(() => setError('Error creating reservation. Please try again.'));
    };

    const handleUpdate = (reservation) => {
        setCurrentReservation(reservation);
        setSelectedActivityId(reservation.activiteId);
        setReservationDate(reservation.dateHeure);
        setShowModal(true);
    };

    const handleUpdateSubmit = (e) => {
        e.preventDefault();

        if (!currentReservation) return;

        const updatedReservation = {
            ...currentReservation,
            DateHeure: reservationDate,
        };

        axios.put(`https://localhost:7126/api/Reservation/${currentReservation.id}`, updatedReservation)
            .then(() => {
                setSuccess('Reservation updated successfully!');
                setShowModal(false);
                axios.get(`https://localhost:7126/api/Reservation/${UtilisateurId}`)
                    .then(response => setReservations(Array.isArray(response.data) ? response.data : []))
                    .catch(() => setError('Failed to update reservations'));
            })
            .catch(() => setError('Error updating reservation. Please try again.'));
    };

    const handleDelete = (reservationId) => {
        setCurrentReservation(reservationId);
        setShowDeleteConfirm(true);
    };

    const confirmDelete = async () => {
        try {
            await axios.delete(`https://localhost:7126/api/Reservation/${currentReservation}`);
            setSuccess('Reservation deleted successfully!');
            setReservations(reservations.filter(reservation => reservation.id !== currentReservation));
            setShowDeleteConfirm(false);
        } catch (error) {
            setError('Error deleting reservation. Please try again.');
        }
    };

    return (
        <Container className="reservations-container">
            <Row className="justify-content-center">
                <Col md={8}>
                    <h2 className="reservations-title">Your Reservations</h2>

                    {error && <Alert variant="danger" className="alert">{error}</Alert>}
                    {success && <Alert variant="success" className="alert">{success}</Alert>}

                    <Card className="reservation-card">
                        <Form onSubmit={handleReservation}>
                            <Row className="mb-3">
                                <Form.Group as={Col} controlId="formActivity">
                                    <Form.Label>Select Activity</Form.Label>
                                    <Form.Control
                                        as="select"
                                        value={selectedActivityId}
                                        onChange={(e) => setSelectedActivityId(e.target.value)}
                                        required
                                    >
                                        <option value="">Choose an activity...</option>
                                        {activities.map(activity => (
                                            <option key={activity.id} value={activity.id}>
                                                {activity.nom} (Capacity: {activity.capaciteMax})
                                            </option>
                                        ))}
                                    </Form.Control>
                                </Form.Group>

                                <Form.Group as={Col} controlId="formDate">
                                    <Form.Label>Select Date and Time</Form.Label>
                                    <Form.Control
                                        type="datetime-local"
                                        value={reservationDate}
                                        onChange={(e) => setReservationDate(e.target.value)}
                                        required
                                    />
                                </Form.Group>
                            </Row>

                            <Button variant="primary" type="submit" className="w-100">
                                Reserve Now
                            </Button>
                        </Form>
                    </Card>
                </Col>
            </Row>

            <Row className="mt-5">
                <Col>
                    {reservations.length === 0 ? (
                        <Alert variant="info">No activities reserved yet.</Alert>
                    ) : (
                        <Row>
                            {reservations.map(reservation => (
                                <Col md={4} key={reservation.id} className="mb-4">
                                    <Card className="shadow-sm border-light">
                                        <Card.Body>
                                            <Card.Title>{reservation.activiteNom}</Card.Title>
                                            <Card.Text>
                                                <strong>Date:</strong> {formatDateTime(reservation.dateHeure)}
                                            </Card.Text>
                                            <Button variant="primary" onClick={() => handleUpdate(reservation)} className="me-2">
                                                Update
                                            </Button>
                                            <Button
                                                variant="link"
                                                className="position-absolute top-0 end-0 p-2"
                                                onClick={() => handleDelete(reservation.id)}
                                                style={{ color: 'red', fontSize: '1.5rem' }}
                                            >
                                                <FaTimes />
                                            </Button>
                                        </Card.Body>
                                    </Card>
                                </Col>
                            ))}
                        </Row>
                    )}
                </Col>
            </Row>

            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Update Reservation</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form onSubmit={handleUpdateSubmit}>
                        <Form.Group controlId="formActiviteNom">
                            <Form.Label>Activity Name</Form.Label>
                            <Form.Control
                                type="text"
                                value={activities.find(activity => activity.id === selectedActivityId)?.nom || ''}
                                readOnly
                            />
                        </Form.Group>
                        <Form.Group controlId="formDateHeure">
                            <Form.Label>Reservation Date</Form.Label>
                            <Form.Control
                                type="datetime-local"
                                value={reservationDate}
                                onChange={(e) => setReservationDate(e.target.value)}
                                required
                            />
                        </Form.Group>
                        <Button variant="primary" type="submit">
                            Update
                        </Button>
                    </Form>
                </Modal.Body>
            </Modal>

            <Modal show={showDeleteConfirm} onHide={() => setShowDeleteConfirm(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Confirm Delete</Modal.Title>
                </Modal.Header>
                <Modal.Body>Are you sure you want to delete this reservation?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowDeleteConfirm(false)}>
                        Cancel
                    </Button>
                    <Button variant="danger" onClick={confirmDelete}>
                        Delete
                    </Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
}

export default Reservations;
