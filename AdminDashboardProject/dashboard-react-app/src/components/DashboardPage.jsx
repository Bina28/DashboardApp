import { useState, useEffect } from "react";
import api from "../api/api";
import { useNavigate } from "react-router-dom";
import ClientRow from "./ClientRow";

export default function DashboardPage() {
  const [clients, setClients] = useState([]);
  const [editingClientId, setEditingClientId] = useState(null);
  const [isAdding, setIsAdding] = useState(false);
  const [newClient, setNewClient] = useState({ name: "", email: "" });

  const navigate = useNavigate();

  const token = localStorage.getItem("token");
  if (!token) {
    return <Navigate to="/" />;
  }

  useEffect(() => {
    async function fetchData() {
      const resultatClients = await api.get("/clients");
      setClients(resultatClients.data);
    }
    fetchData();
  }, []);

  const handleEditStart = (id) => setEditingClientId(id);
  const handleEditCancel = () => setEditingClientId(null);

  const handleClientUpdate = (updatedClient) => {
    setClients((prev) =>
      prev.map((c) => (c.id === updatedClient.id ? updatedClient : c))
    );
    setEditingClientId(null);
  };

  const handleAddClick = () => setIsAdding(true);

  const handleNewClientChange = (e) => {
    const { name, value } = e.target;
    setNewClient((prev) => ({ ...prev, [name]: value }));
  };

  const handleAddCancel = () => {
    setIsAdding(false);
    setNewClient({ name: "", email: "" });
  };

  const handleAddSave = async (e) => {
    e.preventDefault();
    try {
      const response = await api.post("/clients", newClient);
      setClients((prev) => [...prev, response.data]);
      setIsAdding(false);
      setNewClient({ name: "", email: "" });
    } catch (err) {
      alert("Ошибка при добавлении клиента");
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Вы действительно хотите удалить клиента?")) return;
    try {
      await api.delete(`/clients/${id}`);
      setClients((prev) => prev.filter((client) => client.id !== id));
    } catch (err) {
      alert("Ошибка при удалении клиента");
    }
  };

  return (
    <section className="min-h-screen bg-gray-100 py-10 px-4">
      <div className="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-md">
        <h1 className="text-2xl font-bold mb-6 text-center text-gray-800">
          Клиенты
        </h1>

        <button
          onClick={handleAddClick}
          className="mb-4 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
        >
          Добавить клиента
        </button>
        <button
          onClick={() => navigate("/payments-history")}
          className="mb-4 ml-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          История платежей
        </button>

        {isAdding && (
          <form
            onSubmit={handleAddSave}
            className="mb-6 flex flex-col sm:flex-row gap-4 items-center"
          >
            <input
              type="text"
              name="name"
              value={newClient.name}
              onChange={handleNewClientChange}
              placeholder="Имя"
              className="border border-gray-300 rounded-md px-4 py-2 w-full sm:w-auto"
              required
            />
            <input
              type="email"
              name="email"
              value={newClient.email}
              onChange={handleNewClientChange}
              placeholder="Email"
              className="border border-gray-300 rounded-md px-4 py-2 w-full sm:w-auto"
              required
            />
            <div className="flex gap-2">
              <button
                type="submit"
                className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700"
              >
                Сохранить
              </button>
              <button
                type="button"
                onClick={handleAddCancel}
                className="bg-gray-300 text-gray-800 px-4 py-2 rounded-md hover:bg-gray-400"
              >
                Отмена
              </button>
            </div>
          </form>
        )}

        <div className="overflow-x-auto">
          <table className="min-w-full border border-gray-200 text-sm text-left">
            <thead className="bg-gray-200 text-gray-700">
              <tr>
                <th className="p-3">Имя</th>
                <th className="p-3">Email</th>
                <th className="p-3">Баланс</th>
                <th className="p-3">Действия</th>
              </tr>
            </thead>
            <tbody>
              {clients.map((client) => (
                <ClientRow
                  key={client.id}
                  client={client}
                  isEditing={editingClientId === client.id}
                  onEditStart={() => handleEditStart(client.id)}
                  onEditCancel={handleEditCancel}
                  onClientUpdate={handleClientUpdate}
                  onDelete={handleDelete}
                />
              ))}
            </tbody>
          </table>
        </div>

        <button
          onClick={() => window.open("/rates", "_blank")}
          className="mt-6 bg-purple-600 text-white px-5 py-2 rounded-md shadow hover:bg-purple-700 transition-colors duration-300 focus:outline-none focus:ring-2 focus:ring-purple-500"
        >
          Открыть курс
        </button>
      </div>
    </section>
  );
}
