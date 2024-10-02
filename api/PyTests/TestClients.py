import pytest
import json
from unittest.mock import patch, mock_open
from models.clients import Clients

@pytest.fixture
def clients():
    return Clients(root_path="", is_debug=True)

def test_get_clients(clients):
    assert clients.get_clients() == []

def test_get_client(clients):
    clients.data = [{"id": 1, "name": "Client1"}]
    assert clients.get_client(1) == {"id": 1, "name": "Client1"}
    assert clients.get_client(2) is None

def test_add_client(clients):
    client = {"id": 1, "name": "Client1"}
    with patch.object(clients, 'get_timestamp', return_value="2023-01-01"):
        clients.add_client(client)
    assert len(clients.data) == 1
    assert clients.data[0]["created_at"] == "2023-01-01"
    assert clients.data[0]["updated_at"] == "2023-01-01"

def test_update_client(clients):
    clients.data = [{"id": 1, "name": "Client1"}]
    updated_client = {"id": 1, "name": "UpdatedClient"}
    with patch.object(clients, 'get_timestamp', return_value="2023-01-01"):
        clients.update_client(1, updated_client)
    assert clients.data[0]["name"] == "UpdatedClient"
    assert clients.data[0]["updated_at"] == "2023-01-01"

def test_remove_client(clients):
    clients.data = [{"id": 1, "name": "Client1"}]
    clients.remove_client(1)
    assert len(clients.data) == 0

def test_load(clients):
    with patch("builtins.open", mock_open(read_data='[{"id": 1, "name": "Client1"}]')):
        clients.load(is_debug=False)
    assert clients.data == [{"id": 1, "name": "Client1"}]

def test_save(clients):
    clients.data = [{"id": 1, "name": "Client1"}]
    mock_file = mock_open()
    with patch("builtins.open", mock_file):
        clients.save()
    mock_file().write.assert_called_once_with(json.dumps([{"id": 1, "name": "Client1"}]))