// MvrpInit.cpp : Defines the entry point for the console application.
/* Copyright (C) 2015 Xuesong Zhou and junhua chen - All Rights Reserved*/
/*Contact Info: xzhou99@gmail.com, cjh@bjtu.edu.cn*/
/* this code is developed as part of research paper
Finding Optimal Solutions for Vehicle Routing Problem with Pickup and Delivery Services with Time Windows: A Dynamic Programming Approach Based on State-space-time Network Representations
Monirehalsadat Mahmoudi, Xuesong Zhou
http://arxiv.org/abs/1507.02731
*/

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#include "stdafx.h"
#include "MvrpInit.h"
#include "CSVParser.h"

//CWinApp theApp;
//using namespace std;

FILE* g_pFileDebugLog = NULL;
FILE* g_pFileOutputForMainProgram = NULL;
FILE* g_pFileOutput_PP_Similarity = NULL;
FILE* g_pFileOutput_xyt = NULL;

//////////////////the variable fellow first defined in ReadInput function
std::map<int, int> g_internal_node_no_map;
std::map<int, int> g_external_node_id_map;

std::map<string, int> g_internal_agent_no_map;
std::map<int, string> g_external_passenger_id_map;
std::map<int, string> g_external_vehicle_id_map;

float g_passenger_base_profit[_MAX_NUMBER_OF_PASSENGERS] = { 7 };
float g_passenger_request_travel_time_vector[_MAX_NUMBER_OF_PASSENGERS] = { 999 };
int g_accessibility_matrix[_MAX_NUMBER_OF_PASSENGERS][_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_capacity[_MAX_NUMBER_OF_PASSENGERS] = { 1 };
int g_number_of_passengers = 0;

int g_outbound_node_size[_MAX_NUMBER_OF_NODES] = { 0 };
int g_node_passenger_id[_MAX_NUMBER_OF_NODES] = { -1 };
int g_node_passenger_pickup_flag[_MAX_NUMBER_OF_NODES] = { 0 };

int g_outbound_node_id[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_OUTBOUND_NODES];
int g_outbound_link_no[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_OUTBOUND_NODES];
int g_activity_node_flag[_MAX_NUMBER_OF_NODES] = { 0 };
int g_activity_node_ending_time[_MAX_NUMBER_OF_NODES] = { 99999 };
int g_activity_node_starting_time[_MAX_NUMBER_OF_NODES] = { 99999 };

int g_inbound_node_size[_MAX_NUMBER_OF_NODES];
int g_inbound_node_id[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_INBOUND_NODES];
int g_inbound_link_no[_MAX_NUMBER_OF_NODES][_MAX_NUMBER_OF_INBOUND_NODES];


int g_link_free_flow_travel_time[_MAX_NUMBER_OF_LINKS];
float g_link_free_flow_travel_time_float_value[_MAX_NUMBER_OF_LINKS];

float g_link_link_length[_MAX_NUMBER_OF_LINKS];
int g_link_number_of_lanes[_MAX_NUMBER_OF_LINKS];
int g_link_mode_code[_MAX_NUMBER_OF_LINKS];
float g_link_capacity_per_time_interval[_MAX_NUMBER_OF_LINKS];
float g_link_jam_density[_MAX_NUMBER_OF_LINKS];
int g_link_service_code[_MAX_NUMBER_OF_LINKS] = { 0 };


float g_link_speed[_MAX_NUMBER_OF_LINKS];
int g_link_from_node_id[_MAX_NUMBER_OF_LINKS];
int g_link_to_node_id[_MAX_NUMBER_OF_LINKS];


float g_VOIVTT_per_hour[_MAX_NUMBER_OF_VEHICLES];
float g_VOWT_per_hour[_MAX_NUMBER_OF_VEHICLES];

int g_vehicle_path_number_of_nodes[_MAX_NUMBER_OF_VEHICLES] = { 0 };
float g_vehicle_path_cost[_MAX_NUMBER_OF_VEHICLES] = { 0 };  // for vehcile routings

int g_vehicle_origin_node[_MAX_NUMBER_OF_VEHICLES];  // for vehcile routings
int g_vehicle_departure_time_beginning[_MAX_NUMBER_OF_VEHICLES] = { 0 };
int g_vehicle_departure_time_ending[_MAX_NUMBER_OF_VEHICLES];
int g_vehicle_destination_node[_MAX_NUMBER_OF_VEHICLES];
int g_vehicle_arrival_time_ending[_MAX_NUMBER_OF_VEHICLES];
int g_vehicle_arrival_time_beginning[_MAX_NUMBER_OF_VEHICLES] = { 120 };

int g_passenger_origin_node[_MAX_NUMBER_OF_PASSENGERS];  // traveling passengers
int g_passenger_departure_time_beginning[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_departure_time_ending[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_destination_node[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_dummy_destination_node[_MAX_NUMBER_OF_PASSENGERS] = { -1 };

int g_passenger_arrival_time_beginning[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_arrival_time_ending[_MAX_NUMBER_OF_PASSENGERS];
//float g_lamda_vehicle_passenger_assignment_multiplier[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_PASSENGERS] = { 0 };

float g_passenger_request_cancelation_cost[_MAX_NUMBER_OF_PASSENGERS] = { 0 };
int g_vehicle_capacity[_MAX_NUMBER_OF_VEHICLES] = { 1 };

int g_number_of_links = 0;
int g_number_of_nodes = 0;
int g_number_of_physical_nodes = 0;
int g_maximum_number_name_of_nodes = 0;

int g_number_of_time_intervals = 10;//
int g_number_of_vehicles = 0;
int g_number_of_physical_vehicles = 0;
int g_number_of_toll_records = 0;

///////////////////////
///for group_passengers
int g_group_step_x=1;
int g_group_step_y=1;
int g_group_step_t=1;
int g_group_max_x=0;////[0,max_x]
int g_group_max_y=0;////[0,max_y]
int g_group_max_t=0;////[0,max_t]

float g_node_x[_MAX_NUMBER_OF_NODES] = { 0 };
float g_node_y[_MAX_NUMBER_OF_NODES] = { 0 };

int g_passenger_origin_partition[_MAX_NUMBER_OF_PASSENGERS];
int g_passenger_destination_partition[_MAX_NUMBER_OF_PASSENGERS];
////////////////


#pragma region <read input data>


// int -->string
void int2str(const int &int_temp, string &string_temp)
{
	stringstream stream;
	stream << int_temp;
	string_temp = stream.str();
}
// string --> int
int str2int(const string &string_temp)
{
	int a;
	stringstream stream(string_temp);
	stream >> a;
	return a;
}

int g_add_new_node(int passenger_id, int beginning_time = -1, int end_time = -1)
{
	int new_node_internal_id = g_number_of_nodes;
	int new_node_external_number = g_maximum_number_name_of_nodes + 1;

	g_outbound_node_size[new_node_internal_id] = 0;
	g_inbound_node_size[new_node_internal_id] = 0;
	g_node_passenger_id[new_node_internal_id] = passenger_id;
	g_activity_node_flag[new_node_internal_id] = 1;
	g_activity_node_starting_time[new_node_internal_id] = beginning_time;
	g_activity_node_ending_time[new_node_internal_id] = end_time;

	g_internal_node_no_map[new_node_external_number] = new_node_internal_id;
	g_external_node_id_map[new_node_internal_id] = new_node_external_number;

	g_number_of_nodes++;
	g_maximum_number_name_of_nodes++;


	return new_node_internal_id;
}

int g_add_new_link(int from_node_id, int to_node_id, int passenger_id = 0, int travel_time = 1, double link_length = 1, int number_of_lanes = 1, int mode_code = 0,
	int capacity_per_time_interval = 1, double speed = 60)
{
	int new_link_id = g_number_of_links;

	g_outbound_node_id[from_node_id][g_outbound_node_size[from_node_id]] = to_node_id;
	g_outbound_link_no[from_node_id][g_outbound_node_size[from_node_id]] = new_link_id;

	g_outbound_node_size[from_node_id]++;

	g_inbound_node_id[to_node_id][g_inbound_node_size[to_node_id]] = from_node_id;
	g_inbound_link_no[to_node_id][g_inbound_node_size[to_node_id]] = new_link_id;
	g_inbound_node_size[to_node_id]++;


	g_link_from_node_id[new_link_id] = from_node_id;
	g_link_to_node_id[new_link_id] = to_node_id;

	g_link_free_flow_travel_time[new_link_id] = max(1, travel_time);

	g_link_link_length[g_number_of_links] = link_length;
	g_link_number_of_lanes[g_number_of_links] = number_of_lanes;
	g_link_mode_code[g_number_of_links] = mode_code;
	g_link_capacity_per_time_interval[g_number_of_links] = capacity_per_time_interval;
	g_link_speed[g_number_of_links] = speed;
	g_link_service_code[g_number_of_links] = passenger_id;

	g_number_of_links++;
	return g_number_of_links;

}

void g_ReadInputData()
{

	// initialization
	for (int i = 0; i < _MAX_NUMBER_OF_NODES; i++)
	{
		g_outbound_node_size[i] = 0;
		g_inbound_node_size[i] = 0;

	}

	g_number_of_nodes = 0; // initialize  the counter to 0
	g_number_of_links = 0; // initialize  the counter to 0
	g_number_of_time_intervals = 0;

	int interval_node_no = 0;	
	CCSVParser parser;

	//////step 0: read configuration
	if (parser.OpenCSVFile("input_configuration.csv", true))
	{
		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{


			cout << "number of physical nodes = " << g_number_of_nodes << endl;

			g_number_of_physical_nodes = g_number_of_nodes;

			fprintf(g_pFileDebugLog, "number of physical nodes =,%d\n", g_number_of_nodes);
			parser.CloseCSVFile();
		}
	}


	//////////// step 1: read node file 
	if (parser.OpenCSVFile("input_node.csv", true))
	{
		std::map<int, int> node_id_map;

		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{

			string name;

			int node_type;
			int node_id;
			double X;
			double Y;
			if (parser.GetValueByFieldName("node_id", node_id) == false)
				continue;


			if (node_id <= 0 || g_number_of_nodes >= _MAX_NUMBER_OF_NODES)
			{
				cout << "node_id " << node_id << " is out of range" << endl;
				g_ProgramStop();
			}

			g_internal_node_no_map[node_id] = interval_node_no;

			g_external_node_id_map[interval_node_no] = node_id;
			

			g_maximum_number_name_of_nodes = max(g_maximum_number_name_of_nodes, node_id);

			parser.GetValueByFieldName("node_type", node_type);
			parser.GetValueByFieldName("x", X,false);
			parser.GetValueByFieldName("y", Y,false);

			g_node_x[interval_node_no] = X;
			g_node_y[interval_node_no] = Y;

			g_group_max_x =max(X,g_group_max_x);
			g_group_max_y =max(Y,g_group_max_y);

			interval_node_no++;
			g_number_of_nodes++;
			if (g_number_of_nodes % 1000 == 0)
				cout << "reading " << g_number_of_nodes << " physical nodes.. " << endl;

		}

		cout << "number of physical nodes = " << g_number_of_nodes << endl;

		g_number_of_physical_nodes = g_number_of_nodes;

		fprintf(g_pFileDebugLog, "number of physical nodes =,%d\n", g_number_of_nodes);
		parser.CloseCSVFile();

	}

	// step 2: read link file 

	if (parser.OpenCSVFile("input_link.csv", true))
	{
		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{
			int from_node_id = 0;
			int to_node_id = 0;
			if (parser.GetValueByFieldName("from_node_id", from_node_id) == false)
				continue;
			if (parser.GetValueByFieldName("to_node_id", to_node_id) == false)
				continue;

			if (from_node_id <= 0)
			{
				cout << "from_node_id " << from_node_id << " is out of range" << endl;
				g_ProgramStop();
			}

			if (to_node_id <= 0)
			{
				cout << "to_node_id " << to_node_id << " is out of range" << endl;
				g_ProgramStop();
			}

			if (g_internal_node_no_map.find(from_node_id) == g_internal_node_no_map.end())
			{
				//cout << " warning: from_node_id " << from_node_id << " has not been defined in node input file" << endl;
				continue;
			}

			if (g_internal_node_no_map.find(to_node_id) == g_internal_node_no_map.end())
			{
				// cout << "warning  " << to_node_id << " has not been defined in node input file" << endl;
				continue;
			}

			if (g_internal_node_no_map[from_node_id] >= _MAX_NUMBER_OF_NODES)
			{
				cout << "from_node_id " << from_node_id << " is out of range" << endl;
				g_ProgramStop();
			}
			if (g_internal_node_no_map[to_node_id] >= _MAX_NUMBER_OF_NODES)
			{
				cout << "to_node_id " << to_node_id << " is out of range" << endl;
				g_ProgramStop();
			}
			// add the to node id into the outbound (adjacent) node list

			int direction = 1;
			parser.GetValueByFieldName("direction", direction);

			if (direction <= -2 || direction >= 2)
			{
				cout << "direction " << direction << " is out of range" << endl;
				g_ProgramStop();
			}

			for (int link_direction = -1; link_direction <= 1; link_direction += 2)  // called twice; -1 direction , 1 direction 
			{
				if (direction == -1 && link_direction == 1)
					continue; // skip

				if (direction == 1 && link_direction == -1)
					continue; // skip

				// then if  direction == 0 or 2 then create the corresponding link

				int directional_from_node_id = g_internal_node_no_map[from_node_id];
				int directional_to_node_id = g_internal_node_no_map[to_node_id];


				if (link_direction == -1) // reverse direction;
				{
					directional_from_node_id = g_internal_node_no_map[to_node_id];
					directional_to_node_id = g_internal_node_no_map[from_node_id];
				}

				g_outbound_node_id[directional_from_node_id][g_outbound_node_size[directional_from_node_id]] = directional_to_node_id;
				g_outbound_link_no[directional_from_node_id][g_outbound_node_size[directional_from_node_id]] = g_number_of_links;

				g_outbound_node_size[directional_from_node_id]++;
				g_inbound_node_id[directional_to_node_id][g_inbound_node_size[directional_to_node_id]] = directional_from_node_id;
				g_inbound_link_no[directional_to_node_id][g_inbound_node_size[directional_to_node_id]] = g_number_of_links;
				g_inbound_node_size[directional_to_node_id]++;

				float link_length = 1;
				int number_of_lanes = 1;
				int mode_code = 0;
				float capacity_per_time_interval = 1;
				float travel_time = 1.0;
				float speed = 1;
				float jam_density = 200;


				parser.GetValueByFieldName("length", link_length);
				parser.GetValueByFieldName("number_of_lanes", number_of_lanes);
				parser.GetValueByFieldName("mode_code", mode_code);
				parser.GetValueByFieldName("lane_capacity_in_vhc_per_hour", capacity_per_time_interval);

				parser.GetValueByFieldName("speed_limit", speed);
				if (speed >= 70)
					speed = 70;

				if (speed <= 25)
					speed = 25;


				travel_time = max(1, link_length * 60 / max(1, speed));
				parser.GetValueByFieldName("jam_density", jam_density);


				if (travel_time > 1000)
				{
					cout << "travel time of link " << from_node_id << " -> " << to_node_id << " > 1000";
					g_ProgramStop();
				}

				g_link_from_node_id[g_number_of_links] = directional_from_node_id;
				g_link_to_node_id[g_number_of_links] = directional_to_node_id;

				g_link_free_flow_travel_time[g_number_of_links] = max(1, travel_time + 0.5);   // at least 1 min, round to nearest integers
				g_link_free_flow_travel_time_float_value[g_number_of_links] = travel_time;

				g_link_link_length[g_number_of_links] = link_length;
				g_link_number_of_lanes[g_number_of_links] = number_of_lanes;
				g_link_mode_code[g_number_of_links] = mode_code;
				g_link_capacity_per_time_interval[g_number_of_links] = capacity_per_time_interval;
				g_link_speed[g_number_of_links] = speed;

				// increase the link counter by 1
				g_number_of_links++;

				if (g_number_of_links % 1000 == 0)
					cout << "reading " << g_number_of_links << "physical links.. " << endl;

			}

		}

		cout << "number of physical links = " << g_number_of_links << endl;

		fprintf(g_pFileDebugLog, "number of physical links =,%d\n", g_number_of_links);

		parser.CloseCSVFile();
	}


	if (parser.OpenCSVFile("input_agent.csv", true))
	{
		int current_agent_id = -1;
		while (parser.ReadRecord())  // if this line contains [] mark, then we will also read field headers.
		{

			string agent_id;

			parser.GetValueByFieldName("agent_id", agent_id);

			if (agent_id.length() == 0)  // break for empty line
				break;

			//check correctness of input agent_id
			if (str2int(agent_id)>current_agent_id)
			{
				current_agent_id = str2int(agent_id);
			}
			else
			{
				cout << "Illegal agent id sequence. Program stops." << endl;
				g_ProgramStop();
			}

			TRACE("agent_id = %s\n", agent_id.c_str());
			int agent_type = 0;
			parser.GetValueByFieldName("agent_type", agent_type);
			int external_from_node_id;
			int external_to_node_id;
			int from_node_id;
			int to_node_id;


			if (agent_type == 0) //passenger
			{
				int pax_no = g_number_of_passengers + 1;

				g_internal_agent_no_map[agent_id] = pax_no;
				g_external_passenger_id_map[pax_no + 1] = agent_id;


				if (pax_no >= _MAX_NUMBER_OF_PASSENGERS)
				{
					cout << "Agent+ can handle  " << _MAX_NUMBER_OF_PASSENGERS << "passengers" << endl;
					g_ProgramStop();
				}


				parser.GetValueByFieldName("from_node_id", external_from_node_id);
				parser.GetValueByFieldName("to_node_id", external_to_node_id);

				from_node_id = g_internal_node_no_map[external_from_node_id];
				to_node_id = g_internal_node_no_map[external_to_node_id];

				g_passenger_origin_node[pax_no] = from_node_id;
				g_passenger_destination_node[pax_no] = to_node_id;


				parser.GetValueByFieldName("capacity", g_passenger_capacity[pax_no]);
				if (g_passenger_capacity[pax_no] < 0)
				{
					cout << "Passenger data must have values in field capacity in file input_agent.csv!" << endl;
					g_ProgramStop();
				}


				parser.GetValueByFieldName("departure_time_start", g_passenger_departure_time_beginning[pax_no]);
				if (g_passenger_departure_time_beginning[pax_no]<0)
				{
					cout << "Illegal departure time input. Program stops." << endl;
					g_ProgramStop();
				}

				int departure_time_window = 0;
				parser.GetValueByFieldName("departure_time_window", departure_time_window);
				if (departure_time_window<0)
				{
					cout << "Illegal departure time window input. Program stops." << endl;
					g_ProgramStop();
				}
				//g_passenger_departure_time_ending[pax_no] = max(0, departure_time_window);
				//g_passenger_departure_time_ending[pax_no] = max(g_passenger_departure_time_ending[pax_no], c);
				g_passenger_departure_time_ending[pax_no] = g_passenger_departure_time_beginning[pax_no] + departure_time_window;

				parser.GetValueByFieldName("arrival_time_start", g_passenger_arrival_time_beginning[pax_no]);
				if (g_passenger_arrival_time_beginning[pax_no]<0)
				{
					cout << "Illegal arrival time input. Program stops." << endl;
					g_ProgramStop();
				}

				int arrival_time_window = 0;
				parser.GetValueByFieldName("arrival_time_window", arrival_time_window);
				if (arrival_time_window<0)
				{
					cout << "Illegal arrival time window input. Program stops." << endl;
					g_ProgramStop();
				}

				g_passenger_arrival_time_ending[pax_no] = g_passenger_arrival_time_beginning[pax_no] + arrival_time_window;

				//g_passenger_arrival_time_ending[pax_no] = max(g_passenger_arrival_time_ending[pax_no], g_passenger_arrival_time_beginning[pax_no]);

				g_number_of_time_intervals = max(g_passenger_arrival_time_ending[pax_no] + 10, g_number_of_time_intervals);
				g_group_max_t = max(g_passenger_arrival_time_ending[pax_no] + 10,g_group_max_t);

				//adding artificial passenger origin/pickup node id
				int new_artifical_pasenger_origin_id = g_add_new_node(pax_no, g_passenger_departure_time_beginning[pax_no], g_passenger_departure_time_ending[pax_no]);
				g_node_passenger_pickup_flag[new_artifical_pasenger_origin_id] = 1;  // mark this node is a pick-up node

				g_add_new_link(g_passenger_origin_node[pax_no], new_artifical_pasenger_origin_id, pax_no);  // pick up link
				g_add_new_link(new_artifical_pasenger_origin_id, g_passenger_origin_node[pax_no]);

				int new_artifical_pasenger_destination_id = g_add_new_node(pax_no, g_passenger_arrival_time_beginning[pax_no], g_passenger_arrival_time_ending[pax_no]);

				g_passenger_dummy_destination_node[pax_no] = new_artifical_pasenger_destination_id;
				g_add_new_link(g_passenger_destination_node[pax_no], new_artifical_pasenger_destination_id, pax_no *(-1));  // delivery link
				g_add_new_link(new_artifical_pasenger_destination_id, g_passenger_destination_node[pax_no]);

				parser.GetValueByFieldName("base_profit", g_passenger_base_profit[pax_no]);


				g_number_of_passengers++;
			}
			else
			{  // vehicle


				int vehicle_no = g_number_of_vehicles + 1;
				g_external_vehicle_id_map[vehicle_no] = agent_id;


				parser.GetValueByFieldName("from_node_id", external_from_node_id);
				parser.GetValueByFieldName("to_node_id", external_to_node_id);

				from_node_id = g_internal_node_no_map[external_from_node_id];
				to_node_id = g_internal_node_no_map[external_to_node_id];

				g_vehicle_origin_node[vehicle_no] = from_node_id;
				g_vehicle_destination_node[vehicle_no] = to_node_id;

				parser.GetValueByFieldName("departure_time_start", g_vehicle_departure_time_beginning[vehicle_no]);
				int departure_time_window = 0;
				parser.GetValueByFieldName("departure_time_window", departure_time_window);
				g_vehicle_departure_time_ending[vehicle_no] = g_vehicle_departure_time_beginning[vehicle_no] + max(1, departure_time_window);
				g_vehicle_arrival_time_beginning[vehicle_no] = -1;
				parser.GetValueByFieldName("arrival_time_start", g_vehicle_arrival_time_beginning[vehicle_no]);

				if (g_vehicle_arrival_time_beginning[vehicle_no] < 0)
				{
					cout << "Vehicle data must have values in field arrival_time_start in file input_agent.csv!" << endl;
					g_ProgramStop();
				}
				int arrival_time_window = -1;
				parser.GetValueByFieldName("arrival_time_window", arrival_time_window);

				if (arrival_time_window < 0)
				{
					cout << "Vehicle data must have values in field arrival_time_window in file input_agent.csv!" << endl;
					g_ProgramStop();
				}
				g_vehicle_arrival_time_ending[vehicle_no] = g_vehicle_arrival_time_beginning[vehicle_no] + max(1, arrival_time_window);


				g_number_of_time_intervals = max(g_vehicle_arrival_time_ending[vehicle_no] + 10, g_number_of_time_intervals);

				//			if (g_vehicle_arrival_time_ending[vehicle_no] < g_vehicle_departure_time_beginning[vehicle_no] + 60)  // we should use a shortest path travel time to check. 
				//			{
				//				cout << "warning: Arrival time for vehicle " << vehicle_no << " should be " << g_vehicle_departure_time_beginning[vehicle_no] + 120 << endl;
				//				g_vehicle_arrival_time_ending[vehicle_no] = g_vehicle_departure_time_beginning[vehicle_no] + 60;
				////				g_ProgramStop();
				//			}

				g_activity_node_flag[g_vehicle_origin_node[vehicle_no]] = 1;
				g_activity_node_flag[g_vehicle_destination_node[vehicle_no]] = 1;
				g_activity_node_ending_time[g_vehicle_origin_node[vehicle_no]] = g_vehicle_departure_time_ending[vehicle_no];
				g_activity_node_ending_time[g_vehicle_destination_node[vehicle_no]] = g_vehicle_arrival_time_ending[vehicle_no];


				g_vehicle_capacity[vehicle_no] = -1;



				parser.GetValueByFieldName("capacity", g_vehicle_capacity[vehicle_no]);
				if (g_vehicle_capacity[vehicle_no] < 0)
				{
					cout << "Vehicle data must have values in field capacity in file input_agent.csv!" << endl;
					g_ProgramStop();
				}
				parser.GetValueByFieldName("VOIVTT_per_hour", g_VOIVTT_per_hour[vehicle_no]);
				parser.GetValueByFieldName("VOWT_per_hour", g_VOWT_per_hour[vehicle_no]);


				g_number_of_vehicles++;
			}


		}
		parser.CloseCSVFile();
	}
	g_number_of_physical_vehicles = g_number_of_vehicles;

	fprintf(g_pFileDebugLog, "number of passengers=, %d\n", g_number_of_passengers);
	fprintf(g_pFileDebugLog, "number of vehicles =,%d\n", g_number_of_vehicles);
	
	//
	for (int i = 0; i < g_number_of_nodes; i++)
	{
		if (g_outbound_node_size[i] > _MAX_NUMBER_OF_OUTBOUND_NODES - 5)
		{
			cout << "Node number " << g_external_node_id_map[i] << " 's outbound node size is larger than predetermined value. Program stops" << endl;
			g_ProgramStop();
		}
	}
	//

	cout << "read " << g_number_of_nodes << " nodes, " << g_number_of_links << " links" << ", " << g_number_of_passengers << " passengers, " << g_number_of_vehicles << "vehicles" << endl;
	fprintf(g_pFileDebugLog, "network has %d nodes, %d links, %d toll records, %d  passengers, %d vehicles\n",
		g_number_of_nodes, g_number_of_links, g_number_of_toll_records, g_number_of_passengers, g_number_of_vehicles);

	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		fprintf(g_pFileDebugLog, "passenger %d starts from node %d to node %d, start time %d->%d, ending time %d->%d\n",
			p,
			g_external_node_id_map[g_passenger_origin_node[p]],
			g_external_node_id_map[g_passenger_destination_node[p]],
			g_passenger_departure_time_beginning[p],
			g_passenger_departure_time_ending[p],
			g_passenger_arrival_time_beginning[p],
			g_passenger_arrival_time_ending[p]);
	}
	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		fprintf(g_pFileDebugLog, "vehicle %d starts from node %d to node %d, start time %d->%d, ending time %d->%d\n",
			v,
			g_external_node_id_map[g_vehicle_origin_node[v]],
			g_external_node_id_map[g_vehicle_destination_node[v]],
			g_vehicle_departure_time_beginning[v],
			g_vehicle_departure_time_ending[v],
			g_vehicle_arrival_time_beginning[v],
			g_vehicle_arrival_time_ending[v]);

	}
}

#pragma endregion


#pragma region <memory>



////////////////////////////////////////////////////////////////memory 
float** g_node_to_node_shorest_travel_time = NULL;
float** g_vehicle_origin_based_node_travel_time = NULL;
float** g_vehicle_destination_based_node_travel_time = NULL;
void g_allocate_memory_travel_time(int number_of_processors)
{
	int number_of_nodes = g_number_of_nodes + 1;
	int number_of_vehicles = g_number_of_vehicles + 1;

	g_node_to_node_shorest_travel_time = AllocateDynamicArray<float>(number_of_nodes, number_of_nodes, 999);
	g_vehicle_origin_based_node_travel_time = AllocateDynamicArray<float>(number_of_vehicles, number_of_nodes, 0);
	g_vehicle_destination_based_node_travel_time = AllocateDynamicArray<float>(number_of_vehicles, number_of_nodes, 0);

}

void g_free_memory_travel_time(int number_of_processors)
{
	int number_of_nodes = g_number_of_nodes + 1;
	int number_of_vehicles = g_number_of_vehicles + 1;

	exit(0);  // replying on OS to release memory

	DeallocateDynamicArray<float>(g_node_to_node_shorest_travel_time, number_of_nodes, number_of_nodes);
	DeallocateDynamicArray<float>(g_vehicle_origin_based_node_travel_time, number_of_vehicles, number_of_nodes);
	DeallocateDynamicArray<float>(g_vehicle_destination_based_node_travel_time, number_of_vehicles, number_of_nodes);

}

void g_ProgramStop()
{

	cout << "Agent+ Program stops. Press any key to terminate. Thanks!" << endl;
	getchar();
	exit(0);
}

#pragma endregion

#pragma region <shortest path>
//////////////////////////////////////////////

////////////////shortest path

////////////////////////////////////
int g_v_p_shortest_path_maxtrix[_MAX_NUMBER_OF_VEHICLES][_MAX_NUMBER_OF_PASSENGERS];
int g_shortest_path_debugging_flag = 1;


// SEList: Scan List implementation: the reason for not using STL-like template is to avoid overhead associated pointer allocation/deallocation
int g_ListFront;
int g_ListTail;
int g_SENodeList[_MAX_NUMBER_OF_NODES];
void SEList_clear()
{
	g_ListFront = -1;
	g_ListTail = -1;
}

void SEList_push_front(int node)
{
	if (g_ListFront == -1)  // start from empty
	{
		g_SENodeList[node] = -1;
		g_ListFront = node;
		g_ListTail = node;
	}
	else
	{
		g_SENodeList[node] = g_ListFront;
		g_ListFront = node;
	}

}
void SEList_push_back(int node)
{
	if (g_ListFront == -1)  // start from empty
	{
		g_ListFront = node;
		g_ListTail = node;
		g_SENodeList[node] = -1;
	}
	else
	{
		g_SENodeList[g_ListTail] = node;
		g_SENodeList[node] = -1;
		g_ListTail = node;
	}
}

bool SEList_empty()
{
	return(g_ListFront == -1);
}

int SEList_front()
{
	return g_ListFront;
}

void SEList_pop_front()
{
	int tempFront = g_ListFront;
	g_ListFront = g_SENodeList[g_ListFront];
	g_SENodeList[tempFront] = -1;
}


int g_node_status_static_array[_MAX_NUMBER_OF_NODES];
float g_node_label_earliest_arrival_time[_MAX_NUMBER_OF_NODES];
int g_node_static_predecessor[_MAX_NUMBER_OF_NODES];

float g_optimal_label_correcting(int origin_node, int departure_time)
// time-dependent label correcting algorithm with double queue implementation
{
	int internal_debug_flag = 0;

	float total_cost = _MAX_LABEL_COST;
	if (g_outbound_node_size[origin_node] == 0)
	{
		return _MAX_LABEL_COST;
	}

	for (int i = 0; i < g_number_of_nodes; i++) //Initialization for all nodes
	{
		g_node_status_static_array[i] = 0;  // not scanned
		g_node_label_earliest_arrival_time[i] = _MAX_LABEL_COST;
		g_node_static_predecessor[i] = -1;  // pointer to previous NODE INDEX from the current label at current node and time
	}

	//Initialization for origin node at the preferred departure time, at departure time, cost = 0, otherwise, the delay at origin node

	g_node_label_earliest_arrival_time[origin_node] = departure_time;


	SEList_clear();
	SEList_push_back(origin_node);


	while (!SEList_empty())
	{
		int from_node = SEList_front();//pop a node FromID for scanning

		SEList_pop_front();  // remove current node FromID from the SE list
		g_node_status_static_array[from_node] = 0;


		if (g_shortest_path_debugging_flag)
			fprintf(g_pFileDebugLog, "SP: SE node: %d\n", from_node);

		//scan all outbound nodes of the current node
		for (int i = 0; i < g_outbound_node_size[from_node]; i++)  // for each link (i,j) belong A(i)
		{
			int link_no = g_outbound_link_no[from_node][i];
			int to_node = g_outbound_node_id[from_node][i];

			if (to_node == 3)
			{
				int q = 0;
			}

			bool  b_node_updated = false;


			int new_to_node_arrival_time = g_node_label_earliest_arrival_time[from_node] + g_link_free_flow_travel_time[link_no];

			//					if (g_shortest_path_debugging_flag)
			//						fprintf(g_pFileDebugLog, "SP: checking from node %d, to node %d at time %d, FFTT = %d\n",
			//					from_node, to_node, new_to_node_arrival_time,  g_link_free_flow_travel_time[link_no]);


			if (new_to_node_arrival_time < g_node_label_earliest_arrival_time[to_node]) // we only compare cost at the downstream node ToID at the new arrival time t
			{

				//if (g_shortest_path_debugging_flag)
				//	fprintf(g_pFileDebugLog, "SP: updating node: %d from time %d to time %d, current cost: %.2f, from cost %.2f ->%.2f\n",
				//	to_node, t, new_to_node_arrival_time,
				//	g_node_label_cost[from_node][t],
				//	g_node_label_cost[to_node][new_to_node_arrival_time], temporary_label_cost);

				// update cost label and node/time predecessor

				g_node_label_earliest_arrival_time[to_node] = new_to_node_arrival_time;
				g_node_static_predecessor[to_node] = from_node;  // pointer to previous physical NODE INDEX from the current label at current node and time

				b_node_updated = true;


				if (g_node_status_static_array[to_node] != 1)
				{
					/*if (g_shortest_path_debugging_flag)
					fprintf(g_pFileDebugLog, "SP: add node %d into SE List\n",
					to_node);*/

					SEList_push_back(to_node);
					g_node_status_static_array[to_node] = 1;
				}
			}

		}
	}

	for (int i = 0; i <= g_number_of_nodes; i++) //Initialization for all nodes
	{
		g_node_to_node_shorest_travel_time[origin_node][i] = g_node_label_earliest_arrival_time[i];

	}
	return total_cost;

}

void g_generate_travel_time_matrix()
{
	for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
		for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
		{
		g_accessibility_matrix[p1][p2] = 1;
		}

	fprintf(g_pFileDebugLog, "--- travel time and cancelation cost ($)----\n");

	// for each pax
	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		cout << ">>find shortest path tree for pax p = " << p << "..." << endl;

		g_optimal_label_correcting(g_passenger_origin_node[p], g_passenger_departure_time_beginning[p]);//fulfill 

		float earliest_arrival_time = g_node_to_node_shorest_travel_time[g_passenger_origin_node[p]][g_passenger_destination_node[p]];

		if (earliest_arrival_time>1440)
		{
			cout << "Abnormal earliest arrival time. Program stops." << endl;
			g_ProgramStop();
		}

		g_passenger_request_travel_time_vector[p] = max(5, earliest_arrival_time - g_passenger_departure_time_beginning[p]);

		g_passenger_request_cancelation_cost[p] = g_passenger_request_travel_time_vector[p] / 60.0 * 30;
		// from the pax's destination to all the other nodes starting from the earliest arrival time at the d
		g_optimal_label_correcting(g_passenger_destination_node[p], earliest_arrival_time);
		fprintf(g_pFileDebugLog, "pax no.%d, Departure Time = %d (min), Travel Time = %.2f (min), Earliest Arrival Time = %.2f, cost = $%.2f\n",
			p, g_passenger_departure_time_beginning[p],
			g_passenger_request_travel_time_vector[p],
			earliest_arrival_time,
			g_passenger_request_cancelation_cost[p]);

		if (g_passenger_arrival_time_beginning[p] < earliest_arrival_time + 2)
		{
			int existing_arrival_time_window = g_passenger_arrival_time_ending[p] - g_passenger_arrival_time_beginning[p];

			fprintf(g_pFileDebugLog, "modified arrival time window for pax no.%d [%d,%d]  \n",
				p, earliest_arrival_time + 2, earliest_arrival_time + 2 + existing_arrival_time_window);

			g_passenger_arrival_time_beginning[p] = earliest_arrival_time + 2;
			g_passenger_arrival_time_ending[p] = g_passenger_arrival_time_beginning[p] + existing_arrival_time_window;

			float ideal_profit = g_passenger_request_travel_time_vector[p] / 60 * 22;  // 22 dolloar per 60 min 

			if (g_passenger_base_profit[p] < ideal_profit)
			{
				fprintf(g_pFileDebugLog, "modified base profit for pax no.%d from %f to %f  \n",
					p, g_passenger_base_profit[p], ideal_profit);

				g_passenger_base_profit[p] = ideal_profit;
			}


		}

	}
	//check rule 1:
	for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
		for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
		{
		float departure_time_difference = abs(g_passenger_departure_time_beginning[p1] - g_passenger_departure_time_beginning[p2]);
		float minimum_travel_time_p1_p2 = g_node_to_node_shorest_travel_time[g_passenger_origin_node[p1]][g_passenger_origin_node[p2]];
		float minimum_travel_time_p2_p1 = g_node_to_node_shorest_travel_time[g_passenger_origin_node[p2]][g_passenger_origin_node[p1]];

		if (departure_time_difference < min(minimum_travel_time_p1_p2, minimum_travel_time_p2_p1))
		{
			g_accessibility_matrix[p1][p2] = 0;
		}
		}

	// we now have g_node_to_node_shorest_travel_time from all activity node
	fprintf(g_pFileDebugLog, "Least Travel Time for Pax,");
	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		fprintf(g_pFileDebugLog, "Pax %d,", p);

	}
	fprintf(g_pFileDebugLog, "\n,");

	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		fprintf(g_pFileDebugLog, "%.2f,", g_passenger_request_travel_time_vector[p]);

	}
	fprintf(g_pFileDebugLog, "\n");
	fprintf(g_pFileDebugLog, "Modified Vehilce Time,begining, endinging\n");

	
	// calculate shortest path from each vehicle's origin to all nodes

	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		cout << ">>find shortest path tree for vehicle v = " << v << "..." << endl;
		g_optimal_label_correcting(g_vehicle_origin_node[v], g_vehicle_departure_time_beginning[v]);

		for (int i = 0; i < g_number_of_nodes; i++) //Initialization for all nodes
		{
			g_vehicle_origin_based_node_travel_time[v][i] = max(1, g_node_label_earliest_arrival_time[i] - g_vehicle_departure_time_beginning[v]);
			//	fprintf(g_pFileDebugLog, "V %d DN %d =,%f\n", v, i, g_vehicle_origin_based_node_travel_time[v][i]);		
		}

	}
	// calculate shortest path from each vehicle's destination to all nodes

	for (int v = 1; v <= g_number_of_vehicles; v++)
	{
		cout << ">>find shortest path tree for vehicle v = " << v << "..." << endl;
		g_optimal_label_correcting(g_vehicle_destination_node[v], 0);

		for (int p = 1; p < g_number_of_passengers; p++) //Initialization for all nodes
		{
			//g_vehicle_destination_based_node_travel_time[v][i] = max(1, g_node_label_earliest_arrival_time[i]);
			//	fprintf(g_pFileDebugLog, "V %d ON %d =,%f\n", v, i, g_vehicle_destination_based_node_travel_time[v][i]);

			g_vehicle_destination_based_node_travel_time[v][g_passenger_destination_node[p]] = g_node_to_node_shorest_travel_time[g_passenger_destination_node[p]][g_vehicle_destination_node[v]];


		}

	}





	//////////////////////////////
	////calculate v_p_matrix

	int v_p_cost = -1;
	for (int v = 1; v <= g_number_of_physical_vehicles; v++)
	{		
		for (int p = 1; p <= g_number_of_passengers; p++)
		{
			int travel_time_vo_po = g_vehicle_origin_based_node_travel_time[v][g_passenger_origin_node[p]];
			int travel_time_po_pd = g_passenger_request_travel_time_vector[p];
			int travel_time_pd_vd = g_vehicle_destination_based_node_travel_time[v][g_passenger_destination_node[p]];

			bool isReachable = true;
			v_p_cost = -1;

		

			int t_a = g_vehicle_departure_time_beginning[v] + travel_time_vo_po;
			int t1 = g_passenger_departure_time_beginning[p];
			int t2 = g_passenger_departure_time_ending[p];
			int t_window = g_vehicle_departure_time_ending[v] - g_vehicle_departure_time_beginning[v];

			if (t_a>t2)   ////t_a + t_window<t1 
			{
				isReachable = false;
				g_v_p_shortest_path_maxtrix[v][p] = v_p_cost;
				continue;
			}
			else
			{
				if (t_a >= t1)t_a = t_a;
				else t_a = t1;//////////////////////////////////check    t_a=t1;
			}
			///////part 2: from p_o->p_d
			t_window = g_passenger_departure_time_ending[p] - t_a;
			t_a = t_a + travel_time_po_pd;
			t1 = g_passenger_arrival_time_beginning[p];
			t2 = g_passenger_arrival_time_ending[p];

			if (t_a>t2)   /////t_a + t_window<t1 || 
			{
				isReachable = false;
				g_v_p_shortest_path_maxtrix[v][p] = v_p_cost;
				continue;
			}
			else
			{
				if (t_a >= t1)t_a = t_a;
				else t_a = t1;
			}
			///////part 3: from p_d->v_d
			t_window = g_passenger_arrival_time_ending[p] - t_a;
			t_a = t_a + travel_time_pd_vd;
			t1 = g_vehicle_arrival_time_beginning[v];
			t2 = g_vehicle_arrival_time_ending[v];

			//if (t_a>t2)
			if (t_a>t2)            //////t_a + t_window<t1 || 
			{
				isReachable = false;
				g_v_p_shortest_path_maxtrix[v][p] = v_p_cost;
				continue;
			}
			else
			{
				if (t_a >= t1)t_a = t_a;
				else t_a = t1;
			}
			int jurnalTime= t_a - g_vehicle_departure_time_beginning[v];
			int runningTime=travel_time_vo_po + travel_time_po_pd + travel_time_pd_vd;
			if (isReachable)v_p_cost = runningTime;
			else v_p_cost = -1;
			g_v_p_shortest_path_maxtrix[v][p] = v_p_cost;
		}
	}

	/////////print
	fprintf(g_pFileDebugLog, "\ng_v_p_shortest_path_maxtrix[v,p],");
	fprintf(g_pFileDebugLog, "\nv,");
	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		fprintf(g_pFileDebugLog, "p%d,", p);
	}
	for (int v = 1; v <= g_number_of_physical_vehicles; v++)
	{
		fprintf(g_pFileDebugLog, "\n%d,", v);
		for (int p = 1; p <= g_number_of_passengers; p++)
		{
			if (g_v_p_shortest_path_maxtrix[v][p] == -1)fprintf(g_pFileDebugLog, "-,");
			else	fprintf(g_pFileDebugLog, "%d,", g_v_p_shortest_path_maxtrix[v][p]);
		}
	}

}


#pragma endregion

void g_generate_file_for_main_programe()
{

	fprintf(g_pFileOutputForMainProgram, "%d,%d,%d\n", g_number_of_physical_vehicles, g_number_of_passengers, -1);

	/////begin d(v,p)
	for (int v = 1; v <= g_number_of_physical_vehicles; v++)
		for (int p = 1; p <= g_number_of_passengers; p++)
		{
		float cost = 9999;
		if (g_v_p_shortest_path_maxtrix[v][p] != -1)cost = g_v_p_shortest_path_maxtrix[v][p];
		fprintf(g_pFileOutputForMainProgram, "%d,%d,%f\n", v, p, cost);
		}
}


//////////////////


int g_get_xyt_order(int x,int y,int t,int N_X,int N_Y,int N_T)
{
	int xyt_order = 0;
	int x_order = 0, y_order = 0, t_order = 0;
	for (int n = 1; n <= N_X;n++)
	{
		if (x > (n - 1)*g_group_step_x && x <= n*g_group_step_x)
		{
			x_order = n; break;
		}
	}
	for (int n = 1; n <= N_Y; n++)
	{
		if (y > (n - 1)*g_group_step_y && y <= n*g_group_step_y)
		{
			y_order = n; break;
		}
	}
	for (int n = 1; n <= N_T; n++)
	{
		if (t > (n - 1)*g_group_step_t && t <= n*g_group_step_t)
		{
			t_order = n; break;
		}
	}
	xyt_order = (t_order - 1)*(N_X*N_Y) + (y_order - 1)*N_X + x_order;
	return xyt_order;
}

void g_partition_xyt()
{
	g_group_step_x = 30;
	g_group_step_y = 30;
	g_group_step_t = 600;

	int N_X = ceil(g_group_max_x / (float)g_group_step_x);
	int N_Y = ceil(g_group_max_y / (float)g_group_step_y);
	int N_T = ceil(g_group_max_t / (float)g_group_step_t);

	for (int p = 1; p <= g_number_of_passengers; p++)
	{
		int x_o = g_node_x[g_passenger_origin_node[p]];
		int y_o = g_node_y[g_passenger_origin_node[p]];
		int t_o = g_passenger_departure_time_beginning[p];
		int xyt_order_o = g_get_xyt_order(x_o, y_o, t_o, N_X, N_Y, N_T);
		g_passenger_origin_partition[p] = xyt_order_o;

		int x_d = g_node_x[g_passenger_destination_node[p]];
		int y_d = g_node_y[g_passenger_destination_node[p]];
		int t_d = g_passenger_arrival_time_beginning[p];
		int xyt_order_d = g_get_xyt_order(x_d, y_d, t_d, N_X, N_Y, N_T);
		g_passenger_destination_partition[p] = xyt_order_d;
	}

	int xxx = 0;

}

void g_computer_similarity_passenger()
{
	float g_p_p_similarity[_MAX_NUMBER_OF_PASSENGERS][_MAX_NUMBER_OF_PASSENGERS] = { 0 };
	for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
	{
		float x_p1_o = g_node_x[g_passenger_origin_node[p1]];
		float y_p1_o = g_node_y[g_passenger_origin_node[p1]];
		float t_p1_o = g_passenger_departure_time_beginning[p1];

		float x_p1_d = g_node_x[g_passenger_destination_node[p1]];
		float y_p1_d = g_node_y[g_passenger_destination_node[p1]];
		float t_p1_d = g_passenger_arrival_time_beginning[p1];

		for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
		{			

			float x_p2_o = g_node_x[g_passenger_origin_node[p2]];
			float y_p2_o = g_node_y[g_passenger_origin_node[p2]];
			float t_p2_o = g_passenger_departure_time_beginning[p2];			

			float x_p2_d = g_node_x[g_passenger_destination_node[p2]];
			float y_p2_d = g_node_y[g_passenger_destination_node[p2]];
			float t_p2_d = g_passenger_arrival_time_beginning[p2];

			float similarity_o = sqrt((x_p1_o - x_p2_o)*(x_p1_o - x_p2_o) + (y_p1_o - y_p2_o)*(y_p1_o - y_p2_o) + (t_p1_o - t_p2_o)* (t_p1_o - t_p2_o));
			float similarity_d = sqrt((x_p1_d - x_p2_d)*(x_p1_d - x_p2_d) + (y_p1_d - y_p2_d)*(y_p1_d - y_p2_d) + (t_p1_d - t_p2_d)* (t_p1_d - t_p2_d));

			g_p_p_similarity[p1][p2] = max(similarity_o, similarity_d);
		}
		////out xyt
		fprintf(g_pFileOutput_xyt, "p,%d,%f,%f,%f,%f,%f,%f\n", p1, x_p1_o, y_p1_o, t_p1_o, x_p1_d, y_p1_d, t_p1_d);

	}

	for (int v = 1; v <= g_number_of_physical_vehicles; v++)
	{
		float x_v_o = g_node_x[g_vehicle_origin_node[v]];
		float y_v_o = g_node_y[g_vehicle_origin_node[v]];
		float t_v_o = g_vehicle_departure_time_beginning[v];

		float x_v_d = g_node_x[g_vehicle_destination_node[v]];
		float y_v_d = g_node_y[g_vehicle_destination_node[v]];
		float t_v_d = g_vehicle_arrival_time_beginning[v];
		////out xyt
		fprintf(g_pFileOutput_xyt, "v,%d,%f,%f,%f,%f,%f,%f\n", v, x_v_o, y_v_o, t_v_o, x_v_d, y_v_d, t_v_d);
	}


	//////////////////////////
	fprintf(g_pFileOutput_PP_Similarity, "%d,%d,%d\n", g_number_of_physical_vehicles, g_number_of_passengers, -1);

	/////begin c(p,p')
	for (int p1 = 1; p1 <= g_number_of_passengers; p1++)
		for (int p2 = 1; p2 <= g_number_of_passengers; p2++)
		{		
		fprintf(g_pFileOutput_PP_Similarity, "%d,%d,%f\n", p1, p2, g_p_p_similarity[p1][p2]);
		}

	///////////////


}

///////////////////////////////////////////////////////////////////////////////////////

                 /////new 
/////////////////////////////////////////////////////////////////////////////////////
int _tmain(int argc, _TCHAR* argv[])
{
	int nRetCode = 0;
	HMODULE hModule = ::GetModuleHandle(NULL);

	if (hModule != NULL)
	{
		// initialize MFC and print and error on failure
		if (!AfxWinInit(hModule, NULL, ::GetCommandLine(), 0))
		{
			// TODO: change error code to suit your needs
			_tprintf(_T("Fatal Error: MFC initialization failed\n"));
			nRetCode = 1;
		}
		else
		{
			// TODO: code your application's behavior here.
		}
	}
	else
	{
		// TODO: change error code to suit your needs
		_tprintf(_T("Fatal Error: GetModuleHandle failed\n"));
		nRetCode = 1;
	}

	g_pFileDebugLog = fopen("Debug_init.txt", "w");
	if (g_pFileDebugLog == NULL)
	{
		cout << "File Debug_init.txt cannot be opened." << endl;
		g_ProgramStop();
	}

	g_pFileOutputForMainProgram = fopen("trans_A_set_distance_v_p.csv", "w");
	if (g_pFileOutputForMainProgram == NULL)
	{
		cout << "File trans_A_set_distance_v_p.csv cannot be opened." << endl;
		g_ProgramStop();
	}
	fprintf(g_pFileOutputForMainProgram, "v,p,value\n"); // header


	g_pFileOutput_PP_Similarity = fopen("trans_A_set_similarity_p_p.csv", "w");
	if (g_pFileOutput_PP_Similarity == NULL)
	{
		cout << "File trans_A_set_similarity_p_p.csv cannot be opened." << endl;
		g_ProgramStop();
	}
	fprintf(g_pFileOutput_PP_Similarity, "p1,p2,value\n"); // header


	g_pFileOutput_xyt = fopen("trans_A_set_xyt.csv", "w");
	if (g_pFileOutput_xyt == NULL)
	{
		cout << "File trans_A_set_xyt.csv cannot be opened." << endl;
		g_ProgramStop();
	}
	fprintf(g_pFileOutput_xyt, "type,p/v,o_x,o_y,o_t,d_x,d_y,d_t\n"); // header

	g_ReadInputData();

	//g_partition_xyt();
	g_computer_similarity_passenger();

	g_allocate_memory_travel_time(0);
	g_generate_travel_time_matrix();      ////get v_p_shortest_path_maxtrix[v][p] 
	g_generate_file_for_main_programe();


	
	
	fclose(g_pFileDebugLog);
	fclose(g_pFileOutputForMainProgram);
	fclose(g_pFileOutput_PP_Similarity);
	fclose(g_pFileOutput_xyt);

	cout << "End of Optimization " << endl;
	cout << "free memory.." << endl;

	g_free_memory_travel_time(0);
	cout << "done." << endl;

	return nRetCode;
}

